using XLib.Base.Ex;

namespace XLib.Node;

/// <summary>
/// 节点基类
/// </summary>
public abstract class NodeBase
{
    #region 属性

    /// <summary>节点库名称</summary>
    public string NodeLibName { get; set; } = "Inner";

    /// <summary>类型编号</summary>
    public int TypeID { get; set; } = -1;

    /// <summary>版本</summary>
    public string Version { get; set; } = "1.0";

    /// <summary>编号</summary>
    public int ID { get; set; } = -1;

    /// <summary>运行错误</summary>
    public bool RunError { get; set; } = false;

    #region 视图

    /// <summary>坐标</summary>
    public NodePoint Point { get; set; } = new NodePoint();

    /// <summary>颜色</summary>
    public NodeColor Color { get; set; } = new NodeColor();

    /// <summary>图标</summary>
    public string Icon { get; set; } = "Node";

    /// <summary>标题</summary>
    public string Title { get; set; } = "未命名节点";

    #endregion

    /// <summary>引脚组列表：在节点上显示</summary>
    public List<PinGroupBase> PinGroupList { get; set; } = new List<PinGroupBase>();

    /// <summary>属性列表：在属性面板中显示</summary>
    public List<NodeProperty> PropertyList { get; set; } = new List<NodeProperty>();

    /// <summary>节点状态：默认为启用</summary>
    public NodeState State
    {
        get => _state;
        set
        {
            _state = value;
            StateChanged?.Invoke();
        }
    }

    /// <summary>正在编辑</summary>
    public bool InEdit { get; set; } = false;

    #endregion

    #region 委托

    /// <summary>打开进度条</summary>
    public Action<IProgressGetter>? OpenProgressBar { get; set; } = null;

    /// <summary>关闭进度条</summary>
    public Action? CloseProgressBar { get; set; } = null;

    #endregion

    #region 事件

    /// <summary>标题变更</summary>
    public event Action TitleChanged;

    /// <summary>状态变更</summary>
    public event Action StateChanged;

    /// <summary>执行异常</summary>
    public event Action<Exception> ExecuteError;

    /// <summary>参数变更</summary>
    public event Action ParaChanged;

    /// <summary>属性变更</summary>
    public event Action PropertyChanged;

    /// <summary>引脚组列表变更</summary>
    public event Action PinGroupListChanged;

    /// <summary>引脚已断开</summary>
    public event Action<PinBase, PinBase> PinBreaked;

    #endregion

    #region 生命周期

    /// <summary>
    /// 初始化：创建节点实例后调用
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 加载：加载至编辑器后调用
    /// </summary>
    public virtual void Loaded() { }

    /// <summary>
    /// 启用
    /// </summary>
    public virtual void Enable() { }

    /// <summary>
    /// 禁用
    /// </summary>
    public virtual void Disable() { }

    /// <summary>
    /// 卸载：从编辑器移除后调用
    /// </summary>
    public virtual void Unloaded() { }

    /// <summary>
    /// 清理：删除节点时调用
    /// </summary>
    public virtual void Clear() { }

    #endregion

    #region 公开方法

    /// <summary>
    /// 执行节点
    /// </summary>
    public void Execute()
    {
        if (State == NodeState.Disable) return;
        try
        {
            ExecuteNode();
        }
        catch (Exception ex)
        {
            RunError = true;
            Stop();
            ExecuteError.Invoke(ex);
        }
    }


    /// <summary>
    /// 获取引脚
    /// </summary>
    public List<PinBase> GetPin(int groupIndex)
    {
        List<PinBase> result = new List<PinBase>();

        if (PinGroupList.IndexOut(groupIndex)) return result;
        if (PinGroupList[groupIndex].GetInputPin() is PinBase inputPin) result.Add(inputPin);
        if (PinGroupList[groupIndex].GetOutputPin() is PinBase outputPin) result.Add(outputPin);

        return result;
    }

    /// <summary>
    /// 获取全部引脚
    /// </summary>
    public List<PinBase> GetAllPin()
    {
        List<PinBase> result = new List<PinBase>();
        foreach (var pinGroup in PinGroupList)
        {
            if (pinGroup.GetInputPin() is PinBase inputPin) result.Add(inputPin);
            if (pinGroup.GetOutputPin() is PinBase outputPin) result.Add(outputPin);
        }
        return result;
    }

    /// <summary>
    /// 查找引脚
    /// </summary>
    public virtual PinBase? FindPin(string nodeVersion, int groupIndex, int pinIndex)
    {
        if (PinGroupList.IndexOut(groupIndex)) return null;

        PinGroupBase group = PinGroupList[groupIndex];
        return group.GetPin(pinIndex);
    }

    /// <summary>
    /// 断开引脚
    /// </summary>
    public void BreakPin(int index)
    {
        foreach (var pin in GetPin(index))
        {
            if (pin.Flow == PinFlow.Input)
            {
                List<PinBase> sourceList = new List<PinBase>(pin.SourceList);
                foreach (var source in sourceList) BreakPin(source, pin);
            }
            else
            {
                List<PinBase> targetList = new List<PinBase>(pin.TargetList);
                foreach (var target in targetList) BreakPin(pin, target);
            }
        }
    }

    /// <summary>
    /// 断开全部引脚
    /// </summary>
    public void BreakAllPin()
    {
        foreach (var pin in GetAllPin())
        {
            if (pin.Flow == PinFlow.Input)
            {
                List<PinBase> sourceList = new List<PinBase>(pin.SourceList);
                foreach (var source in sourceList) BreakPin(source, pin);
            }
            else
            {
                List<PinBase> targetList = new List<PinBase>(pin.TargetList);
                foreach (var target in targetList) BreakPin(pin, target);
            }
        }
    }

    /// <summary>
    /// 获取类型字符串
    /// </summary>
    public abstract string GetTypeString();

    /// <summary>
    /// 获取参数表
    /// </summary>
    public abstract Dictionary<string, string> GetParaDict();

    /// <summary>
    /// 加载参数表
    /// </summary>
    public virtual void LoadParaDict(string version, Dictionary<string, string> paraDict) { }

    /// <summary>
    /// 获取属性表
    /// </summary>
    public virtual Dictionary<string, string> GetPropertyDict() => new Dictionary<string, string>();

    /// <summary>
    /// 加载属性
    /// </summary>
    public virtual void LoadPropertyDict(string version, Dictionary<string, string> propertyDict) { }

    /// <summary>
    /// 克隆
    /// </summary>
    public NodeBase Clone()
    {
        // 克隆派生实例
        NodeBase result = CloneNode();
        // 克隆属性
        result.NodeLibName = NodeLibName;
        result.TypeID = TypeID;
        result.Version = Version;
        result.ID = ID;
        result.Point = new NodePoint(Point.X, Point.Y);
        // 初始化实例
        result.Init();
        // 克隆参数、属性
        result.LoadParaDict(Version, GetParaDict());
        result.LoadPropertyDict(Version, GetPropertyDict());
        // 返回结果
        return result;
    }

    #endregion

    #region 驱动

    public void Start()
    {
        RunError = false;
        Enable();
        State = NodeState.Enable;
    }

    public void Stop()
    {
        Disable();
        State = NodeState.Disable;
    }

    #endregion

    #region 内部方法

    /// <summary>
    /// 设置视图属性：颜色、图标、标题
    /// </summary>
    protected void SetViewProperty(NodeColor color, string icon, string title)
    {
        Color = color;
        Icon = icon;
        Title = title;
    }

    /// <summary>
    /// 初始化引脚组
    /// </summary>
    protected void InitPinGroup()
    {
        foreach (var group in PinGroupList)
        {
            group.Init();
            /*if (group is DataPinGroup dataPinGroup && dataPinGroup.CanInput)
                dataPinGroup.ValueChanged += DataPinGroup_ValueChanged;*/
        }
    }

    /// <summary>
    /// 执行节点
    /// </summary>
    protected virtual void ExecuteNode() { }

    /// <summary>
    /// 获取引脚组
    /// </summary>
    protected T GetPinGroup<T>(int index = 0) where T : PinGroupBase
    {
        if (PinGroupList[index] is T group) return group;
        throw new Exception("获取引脚组失败");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    protected void UpdateData(int index) => ((DataPinGroup)PinGroupList[index]).UpdateValue();

    /// <summary>
    /// 获取数据
    /// </summary>
    protected string GetData(int index) => ((DataPinGroup)PinGroupList[index]).Value;

    /// <summary>
    /// 获取源数据
    /// </summary>
    protected T? GetSourceData<T>(int index)
    {
        if (PinGroupList[index] is TDataPinGroup<T> group) return group.Data;
        return default;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    protected void SetData(int index, string value) => ((DataPinGroup)PinGroupList[index]).Value = value;

    /// <summary>
    /// 设置源数据
    /// </summary>
    protected void SetSourceData<T>(int index, T? value)
    {
        if (PinGroupList[index] is TDataPinGroup<T> group) group.Data = value;
    }

    /// <summary>
    /// 克隆节点
    /// </summary>
    protected abstract NodeBase CloneNode();

    /// <summary>
    /// 引发标题变更
    /// </summary>
    protected void InvokeTitleChanged() => TitleChanged?.Invoke();

    /// <summary>
    /// 引发引脚组列表变更
    /// </summary>
    protected void InvokePinGroupListChanged() => PinGroupListChanged?.Invoke();

    /// <summary>
    /// 引发执行异常
    /// </summary>
    protected void InvokeExecuteError(Exception ex) => ExecuteError?.Invoke(ex);

    #endregion

    #region 引脚组事件

    private void DataPinGroup_ValueChanged() => ParaChanged?.Invoke();

    #endregion

    #region 私有方法

    /// <summary>
    /// 断开引脚
    /// </summary>
    private void BreakPin(PinBase source, PinBase target)
    {
        source.TargetList.Remove(target);
        target.SourceList.Remove(source);
        PinBreaked?.Invoke(source, target);
    }

    #endregion

    #region 属性字段

    private NodeState _state = NodeState.Enable;

    #endregion
}