using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text;
using Ymiot.Core.Utils;

namespace Ymiot.Core.Miio;

/// <summary>
/// 米家接口客户端
/// </summary>
public class MiioClient
{
    private const string DefaultApiUrl = "https://api.io.mi.com/app";

    private string UserAgent { get; }

    private string AgentId { get; }

    private string? Sid { get; set; }

    private string? UserName { get; set; }

    private string? Password { get; set; }

    /// <summary>
    /// 登录信息
    /// </summary>
    public LoginInfo? LoginInfo { get; private set; }

    public MiioClient(string? sid = default, string? userName = default, string? password = default)
    {
        AgentId = RandomUtil.RandomAgentId();
        UserAgent = $"Android-7.1.1-1.0.0-ONEPLUS A3010-136-{AgentId} APP/xiaomi.smarthome APPV/62830";
        Sid = sid;
        UserName = userName;
        Password = password;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="sid">SID</param>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<LoginInfo> LoginAsync(
        string sid,
        string username,
        string password,
        CancellationToken token = default)
    {
        const string accountBase = "https://account.xiaomi.com";

        LoginInfo = null;
        var restClient = new RestClient();

        var request = new RestRequest($"{accountBase}/pass/serviceLogin");
        request.AddParameter("sid", sid);
        request.AddParameter("_json", true);
        request.AddHeader("User-Agent", UserAgent);

        var response = restClient.Get(request);
        if (!response.IsSuccessful)
        {
            LoginInfo = new LoginInfo(false, response.ErrorMessage ?? "登录失败", sid);
            return LoginInfo;
        }

        var content = JObject.Parse(response.Content![11..]);

        request = new RestRequest($"{accountBase}/pass/serviceLoginAuth2");
        request.AddHeader("User-Agent", UserAgent);
        request.AddParameter("user", username);
        request.AddParameter("hash", SecurityUtil.GetMd5(password).ToUpper());
        request.AddParameter("callback", content.GetString("callback"));
        request.AddParameter("sid", content.GetString("sid"));
        request.AddParameter("qs", content.GetString("qs"));
        request.AddParameter("_sign", content.GetString("_sign"));
        request.AddQueryParameter("_json", "true");

        response = await restClient.PostAsync(request, token);
        if (!response.IsSuccessful)
        {
            LoginInfo = new LoginInfo(false, response.ErrorMessage ?? "登录失败", sid);
            return LoginInfo;
        }

        content = JObject.Parse(response.Content![11..]);
        if (content.GetValue("code")?.Value<int>() != 0)
        {
            LoginInfo = new LoginInfo(false, content.GetString("desc") ?? "登录失败", sid);
            return LoginInfo;
        }

        var deviceId = response.Cookies?.FirstOrDefault(c => c.Name == "deviceId")?.Value ?? AgentId;
        var nonce = content.GetString("nonce")!;
        var location = content.GetString("location")!;
        var userId = content.GetString("userId")!;
        var securityToken = content.GetString("ssecurity")!;

        if (sid != "xiaomiio")
        {
            var clientSign = GetClientSign(nonce, securityToken);
            location = $"{location}&clientSign={clientSign}";
        }

        request = new RestRequest(location);
        request.AddOrUpdateHeader("content-type", "application/x-www-form-urlencoded");

        response = restClient.Get(request);
        if (!response.IsSuccessful)
        {
            LoginInfo = new LoginInfo(false, response.ErrorMessage ?? "登录失败", sid, userId, SecurityToken: securityToken);
            return LoginInfo;
        }

        UserName = username;
        Password = password;

        var serviceToken = response.Cookies?.FirstOrDefault(c => c.Name == "serviceToken")?.Value;
        var successful = !string.IsNullOrEmpty(serviceToken);
        LoginInfo = new LoginInfo(successful, successful ? "登录成功" : "未获取到serviceToken", sid, userId, deviceId, serviceToken, securityToken);
        return LoginInfo;
    }

    public void Logout()
    {
        LoginInfo = null;
    }

    private static string GetClientSign(string nonce, string securityToken)
    {
        var original = $"nonce={nonce}&{securityToken}";
        var sha1 = SecurityUtil.GetSha1(original);
        var base64 = SecurityUtil.GetBase64(sha1);
        return WebUtility.UrlEncode(base64);
    }

    private async Task CheckLoginInfo(LoginInfo? loginInfo, CancellationToken token)
    {
        var count = 0;
        while (loginInfo is not { IsSuccessful: true } || string.IsNullOrEmpty(loginInfo.ServiceToken))
        {
            if (count >= 3)
                throw new Exception("登录失败，请重新登录");

            if (string.IsNullOrWhiteSpace(Sid) || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                throw new Exception("未登录，请登录后再试");

            await Task.Delay(1000, token);
            loginInfo = await LoginAsync(Sid, UserName, Password, token);
            count++;
        }
    }

    private static string GenerateNonce()
    {
        return RandomUtil.Random(16);
    }

    private static string GenerateSignedNonce(string secret, string nonce)
    {
        var data = new List<byte>();
        data.AddRange(SecurityUtil.DecodeBase64(secret));
        data.AddRange(SecurityUtil.DecodeBase64(nonce));
        var sha256 = SecurityUtil.GetSha256(data.ToArray());
        return SecurityUtil.GetBase64(sha256);
    }

    private static string GenerateSignature(
        string url,
        string signedNonce,
        string nonce,
        string data)
    {
        var sign = $"{url}&{signedNonce}&{nonce}&data={data}";
        var hmacSha256 = SecurityUtil.GetHmacSha256(SecurityUtil.DecodeBase64(signedNonce), sign);
        return SecurityUtil.GetBase64(hmacSha256);
    }

    private static string Sha1Sign(
        string method,
        string url,
        IReadOnlyDictionary<string, object> data,
        string nonce)
    {
        var path = url;
        var index = path.IndexOf("/app/", StringComparison.CurrentCulture);
        if (index >= 0)
            path = path[(index + 4)..];

        var arr = new List<string> { method.ToUpper(), path };
        foreach (var (key, value) in data)
        {
            arr.Add($"{key}={value}");
        }
        arr.Add(nonce);
        var sha1 = SecurityUtil.GetSha1(string.Join("&", arr));
        return SecurityUtil.GetBase64(sha1);
    }

    private static string EncryptData(string pwd, string value)
    {
        var password = SecurityUtil.DecodeBase64(pwd);
        var data = Encoding.UTF8.GetBytes(value);
        var rc4 = new Rc4(password).Init1024().Crypt(data);
        return SecurityUtil.GetBase64(rc4);
    }

    private static string DecryptData(string pwd, string value)
    {
        var password = SecurityUtil.DecodeBase64(pwd);
        var data = SecurityUtil.DecodeBase64(value);
        var rc4 = new Rc4(password).Init1024().Crypt(data);
        return Encoding.UTF8.GetString(rc4);
    }

    private RestRequest GetApiRequest(string url)
    {
        if (!url.StartsWith("https:") && !url.StartsWith("http:"))
        {
            url = $"{DefaultApiUrl}/{url.TrimStart('/')}";
        }

        var request = new RestRequest(url);
        request.AddOrUpdateHeader("User-Agent", UserAgent);
        request.AddOrUpdateHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddOrUpdateHeader("x-xiaomi-protocal-flag-cli", "PROTOCAL-HTTP2");
        request.AddOrUpdateHeader("Cookie", $"PassportDeviceId={LoginInfo!.DeviceId};userId={LoginInfo.UserId};yetAnotherServiceToken={LoginInfo.ServiceToken};serviceToken={LoginInfo.ServiceToken};channel=MI_APP_STORE;");

        return request;
    }

    public async Task<JToken?> RequestMiotApiAsync(
        string api,
        object? data,
        string method = "POST",
        bool crypt = true,
        CancellationToken token = default)
    {
        var parameters = new Dictionary<string, object>();
        if (data != null)
            parameters.Add("data", JsonHelper.SerializeObject(data)!);

        var raw = LoginInfo!.Sid != "xiaomiio";
        string content;
        if (raw)
            content = await RequestRawAsync(api, parameters, method, token);
        else if (crypt)
            content = await RequestRc4Async(api, parameters, method, token);
        else
            content = await PostAsync(api, parameters, token);

        var json = JObject.Parse(content);
        var code = json.GetInt("code");
        if (code == 3)
        {
            Logout();
            throw new Exception("登录信息无效");
        }
        return json["result"];
    }

    private async Task<string> PostAsync(
        string api,
        Dictionary<string, object> parameters,
        CancellationToken token = default)
    {
        await CheckLoginInfo(LoginInfo, token);

        var data = parameters["data"] as string;

        var nonce = GenerateNonce();
        var signedNonce = GenerateSignedNonce(LoginInfo!.SecurityToken!, nonce);
        var signature = GenerateSignature(api, signedNonce, nonce, data!);

        parameters.Add("_nonce", nonce);
        parameters.Add("signature", signature);

        var request = GetApiRequest(api);

        AddParameters(request, parameters);

        var restClient = new RestClient();

        var response = await restClient.PostAsync(request, token);
        if (!response.IsSuccessful)
            throw new Exception(response.ErrorMessage);

        if (string.IsNullOrEmpty(response.Content))
            throw new Exception("内容为空");

        return response.Content;
    }

    private async Task<string> RequestRawAsync(
        string api,
        Dictionary<string, object> parameters,
        string method = "POST",
        CancellationToken token = default)
    {
        var request = GetApiRequest(api);
        AddParameters(request, parameters);

        request.Method = method == "GET" ? Method.Get : Method.Post;

        var restClient = new RestClient();
        var response = await restClient.ExecuteAsync(request, token);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            Logout();

        if (!response.IsSuccessful)
            throw new Exception(response.ErrorMessage);

        var content = response.Content;

        if (string.IsNullOrEmpty(content))
            throw new Exception("内容为空");

        if (content.Contains("error") || content.Contains("invalid"))
            throw new Exception($"请求出错: {content}");

        return content;
    }

    private async Task<string> RequestRc4Async(
        string api,
        Dictionary<string, object> parameters,
        string method = "POST",
        CancellationToken token = default)
    {
        var request = GetApiRequest(api);
        request.AddOrUpdateHeader("MIOT-ENCRYPT-ALGORITHM", "ENCRYPT-RC4");
        request.AddOrUpdateHeader("Accept-Encoding", "identity");

        parameters = GetRc4Params(method, request.Resource, parameters);
        AddParameters(request, parameters);

        var signedNonce = GenerateSignedNonce(LoginInfo!.SecurityToken!, parameters["_nonce"].ToString()!);

        request.Method = method == "GET" ? Method.Get : Method.Post;

        var restClient = new RestClient();
        var response = await restClient.ExecuteAsync(request, token);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            Logout();

        if (!response.IsSuccessful)
            throw new Exception(response.ErrorMessage);

        var content = response.Content;

        if (string.IsNullOrEmpty(content))
            throw new Exception("内容为空");

        if (content.Contains("error") || content.Contains("invalid"))
            throw new Exception($"请求出错: {content}");

        if (!content.Contains("message"))
        {
            content = DecryptData(signedNonce, content);
        }

        return content;
    }

    private static void AddParameters(RestRequest request, IReadOnlyDictionary<string, object> parameters)
    {
        foreach (var (key, value) in parameters)
        {
            request.AddOrUpdateParameter(key, value.ToString());
        }
    }

    private Dictionary<string, object> GetRc4Params(
        string method,
        string url,
        Dictionary<string, object> parameters)
    {
        var nonce = GenerateNonce();
        var signedNonce = GenerateSignedNonce(LoginInfo!.SecurityToken!, nonce);
        parameters["rc4_hash__"] = Sha1Sign(method, url, parameters, signedNonce);
        foreach (var (key, value) in parameters)
        {
            parameters[key] = EncryptData(signedNonce, value.ToString()!);
        }
        parameters["signature"] = Sha1Sign(method, url, parameters, signedNonce);
        parameters["ssecurity"] = LoginInfo.SecurityToken!;
        parameters["_nonce"] = nonce;
        return parameters;
    }

    /// <summary>
    /// 获取所有设备列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IReadOnlyList<DeviceInfo>?> GetAllDevicesAsync(
        CancellationToken token = default)
    {
        var jToken = await RequestMiotApiAsync(
            "/home/device_list",
            new
            {
                getVirtualModel = true,
                getHuamiDevices = 1,
                get_split_device = false,
                support_smart_home = true
            },
            token: token);
        return jToken?["list"]?.ToObject<List<DeviceInfo>>();
    }

    /// <summary>
    /// 获取所有家庭（包括房间信息）列表
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<HomeInfo>?> GetAllHomesAsync(
        CancellationToken token = default)
    {
        var jToken = await RequestMiotApiAsync(
            "/homeroom/gethome",
            new
            {
                fetch_share_dev = true
            },
            token: token);
        return jToken?["homelist"]?.ToObject<List<HomeInfo>>();
    }

    /// <summary>
    /// 获取所有场景
    /// </summary>
    /// <param name="homeId">家庭编号</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<SceneInfo>?> GetAllScenesAsync(
        string homeId,
        CancellationToken token = default)
    {
        var jToken = await RequestMiotApiAsync(
            "/appgateway/miot/appsceneservice/AppSceneService/GetSceneList",
            new
            {
                home_id = homeId
            },
            token: token);
        return jToken?["scene_info_list"]?.ToObject<List<SceneInfo>>();
    }

    /// <summary>
    /// 获取设备指定属性的值
    /// </summary>
    /// <param name="properties">想要获取的属性</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<DevicePropertyInfo>?> GetDevicePropertyAsync(
        IReadOnlyList<DeviceGetPropertyParam> properties,
        CancellationToken token = default)
    {
        var jToken = await RequestMiotApiAsync(
            "/miotspec/prop/get",
            new Dictionary<string, object>
            {
                { "params", properties }
            },
            token: token);
        return jToken?.ToObject<List<DevicePropertyInfo>>();
    }

    /// <summary>
    /// 设置设备属性值
    /// </summary>
    /// <param name="properties">属性与值</param>
    /// <param name="token"></param>
    public async Task<IReadOnlyList<DeviceSetPropertyResult>?> SetDevicePropertyAsync(
        IReadOnlyList<DeviceSetPropertyParam> properties,
        CancellationToken token = default)
    {
        var jToken = await RequestMiotApiAsync(
            "/miotspec/prop/set",
            new Dictionary<string, object>
            {
                { "params", properties }
            },
            token: token);
        return jToken?.ToObject<List<DeviceSetPropertyResult>>();
    }
}
