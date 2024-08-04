namespace XLib.Node;

/// <summary>
/// 节点类型
/// </summary>
public abstract class NodeType
{
    /// <summary>
    /// 实例化默认类型的节点
    /// </summary>
    public abstract NodeBase NewInstance();

    /// <summary>
    /// 实例化指定类型的节点
    /// </summary>
    public abstract T NewInstance<T>() where T : NodeBase;
}

public class NodeType<SourceType> : NodeType where SourceType : NodeBase, new()
{
    public override string ToString() => typeof(SourceType).Name;

    public override NodeBase NewInstance()
    {
        SourceType result = new SourceType();
        result.Init();
        return result;
    }

    public override ResultType NewInstance<ResultType>()
    {
        SourceType instance = new SourceType();
        if (instance is ResultType result)
        {
            result.Init();
            return result;
        }
        throw new Exception($"无法将“{typeof(SourceType).Name}”转换为“{typeof(ResultType).Name}”");
    }
}