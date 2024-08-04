namespace XLib.Node;

/// <summary>
/// 节点颜色
/// </summary>
public class NodeColor
{
    public NodeColor() { }

    public NodeColor(byte r, byte g, byte b, byte a = 255)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public NodeColor(string hexCode)
    {
        r = Convert.ToByte(hexCode.Substring(0, 2), 16);
        g = Convert.ToByte(hexCode.Substring(2, 2), 16);
        b = Convert.ToByte(hexCode.Substring(4, 2), 16);
    }

#pragma warning disable IDE1006

    public byte a { get; set; } = 255;

    public byte r { get; set; } = 255;

    public byte g { get; set; } = 255;

    public byte b { get; set; } = 255;

#pragma warning restore IDE1006

    public string HexCode => $"{a:X2}{r:X2}{g:X2}{b:X2}";
}