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
}
