using System.Text;

namespace Ymiot.Core.Utils;

public static class RandomUtil
{
    public static string Random(int count)
    {
        const string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var rnd = "";
        for (var i = 0; i < count; i++)
        {
            rnd += str[(int)Math.Floor(System.Random.Shared.NextDouble() * str.Length)];
        }
        return rnd;
    }

    public static string RandomAgentId()
    {
        const string letters = "ABCDEF";
        var sb = new StringBuilder();
        for (var i = 0; i < 13; i++)
        {
            sb.Append(letters[System.Random.Shared.Next(0, 6)]);
        }
        return sb.ToString();
    }
}
