namespace XLib.Node;

/// <summary>
/// 引脚组基类
/// </summary>
public abstract class PinGroupBase
{
    public PinGroupBase(NodeBase node) => OwnerNode = node;

    /// <summary>类型</summary>
    public PinGroupType GroupType { get; set; }

    /// <summary>所属节点</summary>
    public NodeBase OwnerNode { get; set; }

    /// <summary>引脚组索引</summary>
    public int Index => OwnerNode.PinGroupList.IndexOf(this);

    public virtual void Init() { }

    /// <summary>
    /// 获取输入引脚
    /// </summary>
    public abstract PinBase? GetInputPin();

    /// <summary>
    /// 获取输出引脚
    /// </summary>
    public abstract PinBase? GetOutputPin();

    /// <summary>
    /// 获取标题
    /// </summary>
    public abstract string GetTitle();

    /// <summary>
    /// 获取引脚索引
    /// </summary>
    public abstract int GetPinIndex(PinBase pin);

    /// <summary>
    /// 获取引脚
    /// </summary>
    public abstract PinBase? GetPin(int pinIndex);
}

/// <summary>
/// 执行引脚组：表示节点的基础执行流
/// </summary>
public class ExecutePinGroup : PinGroupBase
{
    public ExecutePinGroup(NodeBase node) : base(node)
    {
        GroupType = PinGroupType.Execute;
        InputPin = new ExecutePin(this) { Flow = PinFlow.Input };
        OutputPin = new ExecutePin(this) { Flow = PinFlow.Output };
    }

    public ExecutePinGroup(NodeBase node, string desc) : base(node)
    {
        GroupType = PinGroupType.Execute;
        InputPin = new ExecutePin(this) { Flow = PinFlow.Input };
        OutputPin = new ExecutePin(this) { Flow = PinFlow.Output };
        ExecuteDesc = desc;
    }

    /// <summary>执行描述</summary>
    public string ExecuteDesc { get; set; } = "";

    /// <summary>输入引脚</summary>
    public ExecutePin InputPin { get; set; }

    /// <summary>输出引脚</summary>
    public ExecutePin OutputPin { get; set; }

    public override PinBase? GetInputPin() => InputPin;

    public override PinBase? GetOutputPin() => OutputPin;

    public override string GetTitle() => "";

    public override int GetPinIndex(PinBase pin) => pin == InputPin ? 0 : 1;

    public override PinBase? GetPin(int pinIndex) => pinIndex == 0 ? InputPin : (PinBase)OutputPin;

    public void Execute() => OutputPin.Execute();
}

/// <summary>
/// 数据引脚组：表示节点的一个数据
/// </summary>
public class DataPinGroup : PinGroupBase
{
    public DataPinGroup(NodeBase node) : base(node) => GroupType = PinGroupType.Data;

    public DataPinGroup(NodeBase node, string type, string name, string value) : base(node)
    {
        GroupType = PinGroupType.Data;
        Type = type;
        Name = name;
        DefaultValue = value;
        Value = value;
    }

    #region 属性

    /// <summary>类型</summary>
    public string Type { get; set; } = "";

    /// <summary>名称</summary>
    public string Name { get; set; } = "";

    /// <summary>默认值</summary>
    public string DefaultValue { get; set; } = "";

    /// <summary>当前值</summary>
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            ValueChanged?.Invoke();
        }
    }

    /// <summary>输入框长度</summary>
    public int BoxWidth { get; set; } = 80;

    /// <summary>可读</summary>
    public bool Readable { get; set; } = true;

    /// <summary>可写</summary>
    public bool Writeable { get; set; } = true;

    /// <summary>允许手动输入</summary>
    public bool CanInput { get; set; } = true;

    /// <summary>输入引脚</summary>
    public DataPin? InputPin { get; set; } = null;

    /// <summary>输出引脚</summary>
    public DataPin? OutputPin { get; set; } = null;

    #endregion

    public event Action? ValueChanged;

    #region 基类方法

    public override void Init()
    {
        if (Writeable) InputPin = new DataPin(this) { Flow = PinFlow.Input };
        if (Readable) OutputPin = new DataPin(this) { Flow = PinFlow.Output };
    }

    public override PinBase? GetInputPin() => InputPin;

    public override PinBase? GetOutputPin() => OutputPin;

    public override string GetTitle() => Name;

    public override int GetPinIndex(PinBase pin)
    {
        if (InputPin != null && pin == InputPin) return 0;
        if (OutputPin != null && pin == OutputPin) return 1;
        throw new Exception("不存在指定引脚");
    }

    public override PinBase? GetPin(int pinIndex)
    {
        if (pinIndex == 0 && InputPin != null) return InputPin;
        if (pinIndex == 1 && OutputPin != null) return OutputPin;
        return null;
    }

    #endregion

    /// <summary>
    /// 更新值
    /// </summary>
    public void UpdateValue()
    {
        // 如果连接了源，则更新为源的值
        if (InputPin != null && InputPin.SourceList.Count > 0)
        {
            ((DataPinGroup)InputPin.SourceList[0].OwnerGroup).UpdateValue();
            Value = ((DataPinGroup)InputPin.SourceList[0].OwnerGroup).Value;
        }
    }

    private string _value = "";
}

public class TDataPinGroup<TData> : DataPinGroup
{
    public TDataPinGroup(NodeBase node) : base(node) { }

    public TDataPinGroup(NodeBase node, string type, string name, string value) : base(node, type, name, value) { }

    public TData? Data { get; set; }
}

/// <summary>
/// 动作引脚组：表示节点的行为
/// </summary>
public class ActionPinGroup : PinGroupBase
{
    public ActionPinGroup(NodeBase node) : base(node)
    {
        GroupType = PinGroupType.Action;
        OutputPin = new ExecutePin(this) { Flow = PinFlow.Output };
    }

    public ActionPinGroup(NodeBase node, string actionName) : base(node)
    {
        GroupType = PinGroupType.Action;
        OutputPin = new ExecutePin(this) { Flow = PinFlow.Output };
        ActionName = actionName;
    }

    /// <summary>动作名</summary>
    public string ActionName
    {
        get => _actionName;
        set
        {
            _actionName = value;
            ActionNameChanged?.Invoke(value);
        }
    }

    /// <summary>输出引脚</summary>
    public ExecutePin OutputPin { get; set; }

    public Action<string>? ActionNameChanged { get; set; } = null;

    public override PinBase? GetInputPin() => null;

    public override PinBase? GetOutputPin() => OutputPin;

    public override string GetTitle() => ActionName;

    public override int GetPinIndex(PinBase pin) => 1;

    public override PinBase? GetPin(int pinIndex) => pinIndex == 0 ? null : (PinBase)OutputPin;

    public void Invoke() => OutputPin.Execute();

    private string _actionName = "未命名动作";
}

/// <summary>
/// 控件引脚组：用于存放自定义控件。无引脚
/// </summary>
public class ControlPinGroup : PinGroupBase
{
    public ControlPinGroup(NodeBase node) : base(node)
    {
        GroupType = PinGroupType.Control;
    }

    public ControlPinGroup(NodeBase node, object control) : base(node)
    {
        GroupType = PinGroupType.Control;
        ControlInstance = control;
    }

    public object? ControlInstance { get; set; } = null;

    public override PinBase? GetInputPin() => null;

    public override PinBase? GetOutputPin() => null;

    public override string GetTitle() => "";

    public override int GetPinIndex(PinBase pin) => -1;

    public override PinBase? GetPin(int pinIndex) => null;
}