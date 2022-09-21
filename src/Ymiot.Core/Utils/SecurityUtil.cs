using System.Security.Cryptography;
using System.Text;

namespace Ymiot.Core.Utils;

public static class SecurityUtil
{
    public static string GetMd5(string data)
    {
        var md5Bytes = MD5.HashData(Encoding.UTF8.GetBytes(data));
        var result = BitConverter.ToString(md5Bytes).Replace("-", "");
        for (var i = 0; i < 32 - result.Length; i++)
        {
            result = "0" + result;
        }
        return result;
    }

    public static byte[] GetSha1(string data)
    {
        return SHA1.HashData(Encoding.UTF8.GetBytes(data));
    }

    public static byte[] GetSha256(string data)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(data));
    }

    public static byte[] GetSha256(byte[] data)
    {
        return SHA256.HashData(data);
    }

    public static byte[] GetHmacSha256(byte[] key, string data)
    {
        return HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(data));
    }

    public static string GetBase64(byte[] data)
    {
        return Convert.ToBase64String(data);
    }

    public static byte[] DecodeBase64(string data)
    {
        return Convert.FromBase64String(data);
    }

    public static string DecryptRc4(string password, string data)
    {
        var pwd = DecodeBase64(password);
        var dat = DecodeBase64(data);
        var bytes = new Rc4(pwd).Init1024().Crypt(dat);
        return Encoding.UTF8.GetString(bytes);
    }
}
