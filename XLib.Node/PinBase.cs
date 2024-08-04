namespace XLib.Node;

/// <summary>
/// 引脚基类
/// </summary>
public abstract class PinBase
{
    public PinBase(PinGroupBase group) => OwnerGroup = group;

    /// <summary>流向</summary>
    public PinFlow Flow { get; set; } = PinFlow.Input;

    /// <summary>所属引脚组</summary>
    public PinGroupBase OwnerGroup { get; set; }

    /// <summary>源引脚列表</summary>
    public List<PinBase> SourceList { get; set; } = new List<PinBase>();

    /// <summary>目标引脚列表</summary>
    public List<PinBase> TargetList { get; set; } = new List<PinBase>();

    /// <summary>
    /// 添加源引脚
    /// </summary>
    public virtual void AddSource(PinBase source)
    {
        if (SourceList.Contains(source)) return;
        SourceList.Add(source);
    }

    /// <summary>
    /// 添加目标引脚
    /// </summary>
    public void AddTarget(PinBase target)
    {
        if (TargetList.Contains(target)) return;
        TargetList.Add(target);
    }
}

/// <summary>
/// 执行引脚
/// </summary>
public class ExecutePin : PinBase
{
    public ExecutePin(PinGroupBase group) : base(group) { }

    public void Execute()
    {
        // 输入引脚，执行所属节点
        if (Flow == PinFlow.Input)
        {
            OwnerGroup.OwnerNode.Execute();
            return;
        }
        // 输出引脚，执行目标引脚列表
        foreach (var item in TargetList)
            if (item is ExecutePin executePin) executePin.Execute();
    }
}

/// <summary>
/// 数据引脚
/// </summary>
public class DataPin : PinBase
{
    public DataPin(PinGroupBase group) : base(group) { }

    public override void AddSource(PinBase source)
    {
        // 如果已有连接，则先与源断开
        if (SourceList.Count > 0)
        {
            SourceList[0].TargetList.Remove(this);
            SourceList.Clear();
        }
        // 添加新的连接
        SourceList.Add(source);
    }
}