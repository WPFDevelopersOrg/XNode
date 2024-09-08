namespace XLib.Node
{
    /// <summary>
    /// 节点颜色集
    /// </summary>
    public class NodeColorSet
    {
        /// <summary>传感器</summary>
        public static NodeColor Sensor => new NodeColor("DC6464");

        /// <summary>驱动器</summary>
        public static NodeColor Driver => new NodeColor("FF8400");

        /// <summary>事件</summary>
        public static NodeColor Event => new NodeColor("F4E89D");

        /// <summary>函数</summary>
        public static NodeColor Function => new NodeColor("55AAFE");

        /// <summary>流控制</summary>
        public static NodeColor Flow => new NodeColor("B3D465");

        /// <summary>数据</summary>
        public static NodeColor Data => new NodeColor("AAAAAA");

        /// <summary>显示器</summary>
        public static NodeColor Display => new NodeColor("FFFFFF");
    }
}