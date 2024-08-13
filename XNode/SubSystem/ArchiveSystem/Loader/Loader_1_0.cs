using Newtonsoft.Json;
using XLib.Node;
using XNode.SubSystem.ArchiveSystem.Define.Data_1_0;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.NodeLibSystem;
using XNode.SubSystem.WindowSystem;

namespace XNode.SubSystem.ArchiveSystem.Loader
{
    public class Loader_1_0
    {
        public static bool Import(Data_1_0 data, string archiveFilePath)
        {
            try
            {
                LoadNodeList(data);
                LoadConnectLineList(data);
            }
            catch (Exception ex)
            {
                WM.ShowError("加载存档失败：" + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 加载节点列表
        /// </summary>
        private static void LoadNodeList(Data_1_0 data)
        {
            foreach (var nodeString in data.NodeList)
            {
                // 解析节点数据
                NodeData? nodeData = JsonConvert.DeserializeObject<NodeData>(nodeString);
                if (nodeData == null) continue;
                // 解析节点基本数据
                NodeBaseData? nodeBaseData = new NodeBaseData(nodeData.BaseData);
                if (nodeBaseData == null) continue;
                // 创建节点实例
                NodeBase? node;
                if (nodeBaseData.NodeLibName == "Inner")
                    node = NodeLibManager.Instance.CreateNode(nodeBaseData.TypeString);
                else
                    node = NodeLibManager.Instance.CreateNode(nodeBaseData.NodeLibName, nodeBaseData.TypeString);
                // 创建节点失败，则跳过
                if (node == null) continue;
                // 初始化节点
                node.Init();
                // 设置编号与坐标
                node.ID = nodeBaseData.ID;
                node.Point = new NodePoint(nodeBaseData.Point);
                // 加载参数、属性
                node.LoadParaDict(nodeBaseData.Version, nodeData.ParaDict);
                node.LoadPropertyDict(nodeBaseData.Version, nodeData.PropertyDict);
                // 加载节点至编辑器
                WM.Main.Editer.LoadNode(node);
            }
        }

        /// <summary>
        /// 加载连接线列表
        /// </summary>
        private static void LoadConnectLineList(Data_1_0 data)
        {
            foreach (var lineString in data.ConnectLineList)
            {
                // 解析引脚路径
                PinPath startPath = PinPath.ParsePinPath(lineString.Split('-')[0]);
                PinPath endPath = PinPath.ParsePinPath(lineString.Split('-')[1]);
                // 查找引脚
                PinBase? startPin = WM.Main.Editer.FindPin(startPath);
                if (startPin == null) continue;
                PinBase? endPin = WM.Main.Editer.FindPin(endPath);
                if (endPin == null) continue;
                // 连接引脚
                startPin.AddTarget(endPin);
                endPin.AddSource(startPin);
            }
        }
    }
}