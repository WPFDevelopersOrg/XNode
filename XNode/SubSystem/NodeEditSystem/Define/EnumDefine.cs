namespace XNode.SubSystem.NodeEditSystem.Define
{
    /// <summary>
    /// 鼠标命中区域
    /// </summary>
    public enum MouseHitedArea
    {
        /// <summary>空白处</summary>
        Space,
        /// <summary>节点</summary>
        Node,
        /// <summary>引脚</summary>
        Pin,
        /// <summary>连接线</summary>
        ConnectLine,
    }

    /// <summary>
    /// 选择方式
    /// </summary>
    public enum SelectType
    {
        /// <summary>框选</summary>
        Box,
        /// <summary>交叉</summary>
        Cross
    }
}