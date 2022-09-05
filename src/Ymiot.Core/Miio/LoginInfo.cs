namespace Ymiot.Core.Miio;

/// <summary>
/// 登录信息
/// </summary>
/// <param name="IsSuccessful">是否登录成功</param>
/// <param name="Message">信息</param>
/// <param name="Sid">Sid</param>
/// <param name="UserId">用户ID</param>
/// <param name="DeviceId">设备ID</param>
/// <param name="ServiceToken">服务Token</param>
/// <param name="SecurityToken">安全Token</param>
public record LoginInfo(bool IsSuccessful, string Message, string Sid, string UserId = default, string DeviceId = default, string ServiceToken = default, string SecurityToken = default);
