namespace XLib.Node;

/// <summary>
/// 节点坐标
/// </summary>
public class NodePoint
{
    public NodePoint() { }

    public NodePoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public NodePoint(string point)
    {
        X = int.Parse(point.Split(',')[0]);
        Y = int.Parse(point.Split(',')[1]);
    }

    public int X { get; set; } = 0;

    public int Y { get; set; } = 0;

    public override string ToString() => $"{X},{Y}";
}