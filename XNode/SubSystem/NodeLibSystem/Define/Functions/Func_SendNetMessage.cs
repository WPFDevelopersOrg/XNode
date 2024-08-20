using System.Net;
using System.Net.Sockets;
using System.Text;
using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    /// <summary>
    /// 发送网络消息
    /// </summary>
    public class Func_SendNetMessage : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "发送网络消息");

            PinGroupList.Add(new ExecutePinGroup(this, "发送字符串至网络设备"));
            PinGroupList.Add(new DataPinGroup(this, "string", "地址", "127.0.0.1:2400") { BoxWidth = 120, Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "string", "协议", "udp") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "bool", "字节模式", "False") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "string", "消息", "Hello World") { BoxWidth = 200, Readable = false });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 解析地址
            IPEndPoint address = IPEndPoint.Parse(GetData(1));
            // 获取协议
            string protocol = GetData(2).ToUpper();
            // 使用字节模式
            bool byteMode = bool.Parse(GetData(3));
            // 获取消息
            UpdateData(4);
            string message = GetData(4);
            // 消息转字节
            byte[]? messageByte = MessageToByteArray(message, byteMode);
            if (messageByte == null) return;
            // 发送消息
            if (protocol == "UDP")
            {
                UdpClient udpClient = new UdpClient();
                udpClient.Send(messageByte, messageByte.Length, address);

                GetPinGroup<ExecutePinGroup>().Execute();
            }
            else if (protocol == "TCP")
            {
                SynchronizationContext? context = SynchronizationContext.Current;
                if (context == null)
                {
                    context = new SynchronizationContext();
                    SynchronizationContext.SetSynchronizationContext(context);
                }
                Task.Run(() => SendTcpMessageSync(address, messageByte, context));
            }
        }

        public override string GetTypeString() => nameof(Func_SendNetMessage);

        public override Dictionary<string, string> GetParaDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "Address", GetData(1) },
                { "Protocol", GetData(2) },
                { "ByteMode", GetData(3) },
                { "Message", GetData(4) }
            };
            return result;
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            foreach (var para in paraDict)
            {
                switch (para.Key)
                {
                    case "Address":
                        SetData(1, para.Value);
                        break;
                    case "Protocol":
                        SetData(2, para.Value);
                        break;
                    case "ByteMode":
                        SetData(3, para.Value);
                        break;
                    case "Message":
                        SetData(4, para.Value);
                        break;
                }
            }
        }

        protected override NodeBase CloneNode() => new Func_SendNetMessage();

        /// <summary>
        /// 将消息转字节数组
        /// </summary>
        private byte[]? MessageToByteArray(string message, bool byteMode)
        {
            if (!byteMode) return Encoding.UTF8.GetBytes(message);
            try
            {
                // 将消息按空格分割
                string[] byteArraySource = message.Split(' ');
                // 转换为字节数组
                byte[] byteArray = new byte[byteArraySource.Length];
                for (int index = 0; index < byteArraySource.Length; index++)
                    byteArray[index] = Convert.ToByte(byteArraySource[index], 16);
                return byteArray;
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// 异步发送Tcp消息
        /// </summary>
        private void SendTcpMessageSync(IPEndPoint address, byte[] messageByte, SynchronizationContext context)
        {
            try
            {
                // 发送消息
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(address);
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(messageByte, 0, messageByte.Length);
                stream.Close();
                tcpClient.Close();
                // 发送完成，返回调用线程，执行下一个节点
                context.Post(_ => GetPinGroup<ExecutePinGroup>().Execute(), null);
            }
            catch (Exception ex)
            {
                // 发送异常，在调用线程上执行异常处理
                context.Post(_ =>
                {
                    RunError = true;
                    Stop();
                    InvokeExecuteError(ex);
                }, null);
            }
        }
    }
}