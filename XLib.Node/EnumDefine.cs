namespace XLib.Node;

/// <summary>
/// 节点状态
/// </summary>
public enum NodeState
{
    /// <summary>禁用</summary>
    Disable,
    /// <summary>启用</summary>
    Enable,
}

/// <summary>
/// 引脚组类型
/// </summary>
public enum PinGroupType
{
    /// <summary>执行引脚组</summary>
    Execute,
    /// <summary>数据引脚组</summary>
    Data,
    /// <summary>动作引脚组</summary>
    Action,
    /// <summary>控件引脚组</summary>
    Control,
}

/// <summary>
/// 引脚流向
/// </summary>
public enum PinFlow
{
    /// <summary>输入</summary>
    Input,
    /// <summary>输出</summary>
    Output,
}