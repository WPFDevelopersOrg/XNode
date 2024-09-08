using System.IO;
using System.Reflection;
using XLib.Base;
using XLib.Base.Ex;
using XLib.Base.VirtualDisk;
using XLib.Node;
using XNode.SubSystem.NodeLibSystem.Define.Data;
using XNode.SubSystem.NodeLibSystem.Define.Drivers;
using XNode.SubSystem.NodeLibSystem.Define.Events;
using XNode.SubSystem.NodeLibSystem.Define.Flows;
using XNode.SubSystem.NodeLibSystem.Define.Functions;
using XNode.SubSystem.OptionSystem;

namespace XNode.SubSystem.NodeLibSystem
{
    public class NodeLibManager : IManager
    {
        #region 单例

        private NodeLibManager() { }
        public static NodeLibManager Instance { get; } = new NodeLibManager();

        #endregion

        #region 属性

        /// <summary>根文件夹</summary>
        public Folder Root => _nodeLibRoot.Root;

        /// <summary>节点库字典</summary>
        public Dictionary<string, INodeLib> NodeLibDict { get; set; } = new Dictionary<string, INodeLib>();

        #endregion

        #region IManager 方法

        public void Init()
        {
            BuildInnerNodeLib();
            LoadOutsideNodeLib();
        }

        public void Reset() { }

        public void Clear() { }

        #endregion

        #region 公开方法

        /// <summary>
        /// 创建节点
        /// </summary>
        public NodeBase? CreateNode(string typeString)
        {
            return typeString switch
            {
                nameof(Data_Int) => new Data_Int(),
                nameof(Data_Double) => new Data_Double(),
                nameof(Data_String) => new Data_String(),

                nameof(FrameDriver) => new FrameDriver(),
                nameof(TimerDriver) => new TimerDriver(),

                nameof(Event_Keyboard) => new Event_Keyboard(),

                nameof(Flow_If) => new Flow_If(),
                nameof(Flow_LoopByCount) => new Flow_LoopByCount(),
                nameof(Flow_While) => new Flow_While(),
                nameof(Flow_Switch) => new Flow_Switch(),

                nameof(Func_Compare) => new Func_Compare(),
                nameof(Func_NumberToRatio) => new Func_NumberToRatio(),
                nameof(Func_RatioToInt) => new Func_RatioToInt(),
                nameof(Func_SendNetMessage) => new Func_SendNetMessage(),
                nameof(Func_Delay) => new Func_Delay(),
                nameof(Func_CreateThread) => new Func_CreateThread(),
                nameof(Func_Sleep) => new Func_Sleep(),
                nameof(Func_Log) => new Func_Log(),

                _ => null,
            };
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public NodeBase? CreateNode(string libName, string typeString) =>
            NodeLibDict.ContainsKey(libName) ? NodeLibDict[libName].CreateNode(typeString) : null;

        #endregion

        #region 私有方法

        /// <summary>
        /// 构建内置节点库
        /// </summary>
        private void BuildInnerNodeLib()
        {
            // 创建根文件夹
            Folder 内置节点 = _nodeLibRoot.CreateFolder("内置节点".PackToList());
            // 创建一级文件夹
            Folder 驱动节点 = _nodeLibRoot.CreateFolder(内置节点, "驱动节点".PackToList());
            Folder 事件节点 = _nodeLibRoot.CreateFolder(内置节点, "事件节点".PackToList());
            Folder 函数节点 = _nodeLibRoot.CreateFolder(内置节点, "函数节点".PackToList());
            Folder 流控制节点 = _nodeLibRoot.CreateFolder(内置节点, "流控制节点".PackToList());
            Folder 数据节点 = _nodeLibRoot.CreateFolder(内置节点, "数据节点".PackToList());
            // 创建二级文件夹
            Folder 运算函数 = _nodeLibRoot.CreateFolder(函数节点.Path.AppendElement("运算函数"));
            Folder 转换器 = _nodeLibRoot.CreateFolder(函数节点.Path.AppendElement("转换器"));
            Folder 执行控制 = _nodeLibRoot.CreateFolder(函数节点.Path.AppendElement("执行控制"));
            // 创建文件
            _nodeLibRoot.CreateFile(数据节点, "整数", "nt", new NodeType<Data_Int>());
            _nodeLibRoot.CreateFile(数据节点, "小数", "nt", new NodeType<Data_Double>());
            _nodeLibRoot.CreateFile(数据节点, "字符串", "nt", new NodeType<Data_String>());

            _nodeLibRoot.CreateFile(驱动节点, "帧驱动器", "nt", new NodeType<FrameDriver>());
            _nodeLibRoot.CreateFile(驱动节点, "定时驱动器", "nt", new NodeType<TimerDriver>());

            _nodeLibRoot.CreateFile(事件节点, "按键", "nt", new NodeType<Event_Keyboard>());

            _nodeLibRoot.CreateFile(运算函数, "关系运算", "nt", new NodeType<Func_Compare>());
            _nodeLibRoot.CreateFile(转换器, "比例转整数", "nt", new NodeType<Func_RatioToInt>());
            _nodeLibRoot.CreateFile(转换器, "数值转比例", "nt", new NodeType<Func_NumberToRatio>());
            _nodeLibRoot.CreateFile(函数节点, "发送网络消息", "nt", new NodeType<Func_SendNetMessage>());
            _nodeLibRoot.CreateFile(函数节点, "日志", "nt", new NodeType<Func_Log>());

            _nodeLibRoot.CreateFile(执行控制, "多线程执行", "nt", new NodeType<Func_CreateThread>());
            _nodeLibRoot.CreateFile(执行控制, "延迟执行", "nt", new NodeType<Func_Delay>());
            _nodeLibRoot.CreateFile(执行控制, "暂停执行", "nt", new NodeType<Func_Sleep>());

            _nodeLibRoot.CreateFile(流控制节点, "判断", "nt", new NodeType<Flow_If>());
            _nodeLibRoot.CreateFile(流控制节点, "计数循环", "nt", new NodeType<Flow_LoopByCount>());
            _nodeLibRoot.CreateFile(流控制节点, "条件循环", "nt", new NodeType<Flow_While>());
            _nodeLibRoot.CreateFile(流控制节点, "选择执行", "nt", new NodeType<Flow_Switch>());
        }

        /// <summary>
        /// 加载外部节点库
        /// </summary>
        private void LoadOutsideNodeLib()
        {
            // 遍历节点库文件
            foreach (var dllPath in GetAllNodeLibDll())
            {
                // 加载动态库
                Assembly dll = Assembly.LoadFrom(dllPath);
                // 遍历全部类
                foreach (var type in dll.GetTypes())
                {
                    if (typeof(INodeLib).IsAssignableFrom(type))
                    {
                        // 获取单例
                        PropertyInfo? propertyInfo = type.GetProperty("Instance");
                        if (propertyInfo == null) continue;
                        if (propertyInfo.GetValue(null) is not INodeLib instance) continue;
                        // 初始化单例
                        instance.Init();
                        // 保存引用
                        NodeLibDict.Add(instance.Name, instance);
                    }
                }
            }
            // 遍历节点库
            foreach (var libPair in NodeLibDict)
            {
                // 创建根文件夹
                Folder root = _nodeLibRoot.CreateFolder(libPair.Value.Title.PackToList());
                // 加载文件夹
                LoadFolder(root, libPair.Value.LibHarddisk.Root);
            }
        }

        /// <summary>
        /// 获取全部节点库文件
        /// </summary>
        private List<string> GetAllNodeLibDll()
        {
            if (!Directory.Exists(OptionManager.Instance.NodeLibPath)) return new List<string>();

            DirectoryInfo directoryInfo = new DirectoryInfo(OptionManager.Instance.NodeLibPath);
            List<string> result = new List<string>();
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension == ".dll") result.Add(fileInfo.FullName);
            }
            return result;
        }

        /// <summary>
        /// 加载文件夹至目标文件夹
        /// </summary>
        private void LoadFolder(Folder target, Folder oldFolder)
        {
            // 加载文件夹
            foreach (var oldChild in oldFolder.FolderList)
            {
                // 创建子文件夹
                Folder childFolder = new Folder(oldChild.Name, target);
                // 添加子文件夹
                target.FolderList.Add(childFolder);
                // 递归加载
                LoadFolder(childFolder, oldChild);
            }
            // 加载文件
            foreach (var oldFile in oldFolder.FileList)
            {
                // 创建文件
                _nodeLibRoot.CreateFile(target, oldFile.Name, oldFile.Extension, oldFile.Instance);
            }
        }

        #endregion

        #region 字段

        /// <summary>节点库磁盘</summary>
        private readonly Harddisk _nodeLibRoot = new Harddisk();

        #endregion
    }
}