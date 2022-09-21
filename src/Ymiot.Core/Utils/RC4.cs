namespace Ymiot.Core.Utils;

public class Rc4
{
    private int Idx { get; set; }
    private int Jdx { get; set; }
    private byte[] Ksa { get; set; }

    public Rc4(byte[] pwd)
    {
        var cnt = pwd.Length;
        var ksa = new byte[256];
        for (var i = 0; i < ksa.Length; i++)
        {
            ksa[i] = (byte)i;
        }
        var j = 0;
        for (var i = 0; i < 256; i++)
        {
            j = (j + ksa[i] + pwd[i % cnt]) & 255;
            (ksa[i], ksa[j]) = (ksa[j], ksa[i]);
        }

        Ksa = ksa;
        Idx = 0;
        Jdx = 0;
    }

    public byte[] Crypt(byte[] data)
    {
        var ksa = Ksa;
        var i = Idx;
        var j = Jdx;
        var o = new List<byte>();
        foreach (var byt in data)
        {
            i = (i + 1) & 255;
            j = (j + ksa[i]) & 255;
            (ksa[i], ksa[j]) = (ksa[j], ksa[i]);
            o.Add((byte)(byt ^ ksa[(ksa[i] + ksa[j]) & 255]));
        }

        Idx = i;
        Jdx = j;
        return o.ToArray();
    }

    public Rc4 Init1024()
    {
        Crypt(new byte[1024]);
        return this;
    }
}
