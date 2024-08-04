using System.Text.Json;

namespace XLib.Node;

/// <summary>
/// 节点属性
/// </summary>
public class NodeProperty
{
    public NodeProperty() { }

    public NodeProperty(string type, string name, string value)
    {
        Type = type;
        Name = name;
        Value = value;
    }

    /// <summary>类型</summary>
    public string Type { get; set; } = "";

    /// <summary>名称</summary>
    public string Name { get; set; } = "";

    /// <summary>值</summary>
    public string Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            ValueChanged?.Invoke(_value);
        }
    }

    public event Action<string> ValueChanged;

    private string _value = "";
}

/// <summary>
/// 列表属性
/// </summary>
public class ListProperty : NodeProperty
{
    public ListProperty() => Type = "List";

    /// <summary>获取值列表</summary>
    public Func<List<string>>? GetValueList { get; set; } = null;

    public int Index
    {
        get => _index;
        set
        {
            _index = value;
            IndexChanged?.Invoke(_index);
        }
    }

    public Action<int>? IndexChanged { get; set; } = null;

    private int _index = -1;
}

/// <summary>
/// 自定义列表。表示可手动添加、删除的列表
/// </summary>
public class CustomListProperty : NodeProperty
{
    public CustomListProperty() => Type = "CustomList";

    public CustomListProperty(string itemList)
    {
        Type = "CustomList";
        ItemList = JsonSerializer.Deserialize<List<string>>(itemList);
    }

    public List<string> ItemList { get; set; } = new List<string>();

    public Action<string>? ItemAdded { get; set; } = null;

    public Action<int>? ItemRemoved { get; set; } = null;

    public Action<int, string>? ItemChanged { get; set; } = null;

    public override string ToString() => JsonSerializer.Serialize(ItemList);
}