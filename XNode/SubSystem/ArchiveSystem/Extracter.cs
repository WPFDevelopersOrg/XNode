using Newtonsoft.Json;
using XLib.Node;
using XNode.AppTool;
using XNode.SubSystem.ArchiveSystem.Define.Data_1_0;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.WindowSystem;

namespace XNode.SubSystem.ArchiveSystem;

public class Extracter
{
    /// <summary>
    /// 提取存档数据
    /// </summary>
    public static Data_1_0 Extract()
    {
        Data_1_0 result = new Data_1_0();

        FillNodeList(result);
        FillConnectLineList(result);

        return result;
    }

    /// <summary>
    /// 填充节点列表
    /// </summary>
    private static void FillNodeList(Data_1_0 data)
    {
        // 遍历节点
        foreach (var node in WM.Main.Editer.NodeList)
        {
            // 基本数据
            NodeBaseData baseData = new NodeBaseData
            {
                NodeLibName = node.NodeLibName,
                TypeString = node.GetTypeString(),
                Version = node.Version,
                ID = node.ID,
                Point = node.Point.ToString(),
            };
            // 参数与属性
            NodeData nodeData = new NodeData
            {
                BaseData = baseData.ToString(),
                ParaDict = node.GetParaDict(),
                PropertyDict = node.GetPropertyDict(),
            };
            // 添加数据
            data.NodeList.Add(JsonConvert.SerializeObject(nodeData));
        }
    }

    /// <summary>
    /// 填充连接线列表
    /// </summary>
    private static void FillConnectLineList(Data_1_0 data)
    {
        // 连接信息
        Dictionary<PinBase, HashSet<PinBase>> connectInfo = new Dictionary<PinBase, HashSet<PinBase>>();
        // 遍历节点，填充连接信息
        foreach (var node in WM.Main.Editer.NodeList)
        {
            // 遍历全部引脚
            foreach (var pin in node.GetAllPin())
            {
                // 忽略输入引脚与空输出引脚
                if (pin.Flow == PinFlow.Input || pin.TargetList.Count == 0) continue;
                // 添加连接源
                if (!connectInfo.ContainsKey(pin)) connectInfo.Add(pin, new HashSet<PinBase>());
                // 添加连接目标
                foreach (var target in pin.TargetList) connectInfo[pin].Add(target);
            }
        }
        // 遍历连接信息，填充连接线数据
        foreach (var pair in connectInfo)
        {
            PinPath startPath = pair.Key.GetPinPath();
            foreach (var targetPin in pair.Value)
            {
                PinPath endPath = targetPin.GetPinPath();
                ConnectLineData lineData = new ConnectLineData
                {
                    Start = startPath.ToString(),
                    End = endPath.ToString(),
                };
                data.ConnectLineList.Add(lineData.ToString());
            }
        }
    }
}