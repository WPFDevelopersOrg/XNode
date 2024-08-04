using XLib.Base.VirtualDisk;

namespace XLib.Node;

/// <summary>
/// 节点库接口
/// </summary>
public interface INodeLib
{
    /// <summary>库名称：用于标识库</summary>
    public string Name { get; set; }

    /// <summary>库标题：用于显示</summary>
    public string Title { get; set; }

    /// <summary>库磁盘</summary>
    public Harddisk LibHarddisk { get; set; }

    /// <summary>
    /// 初始化库
    /// </summary>
    public void Init();

    /// <summary>
    /// 清理库
    /// </summary>
    public void Clear();

    /// <summary>
    /// 创建节点
    /// </summary>
    public NodeBase? CreateNode(string typeString);
}