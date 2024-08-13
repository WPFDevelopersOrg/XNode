namespace XNode.SubSystem.EventSystem
{
    public enum EventType
    {
        /// <summary>按键按下</summary>
        KeyDown,
        /// <summary>按键松开</summary>
        KeyUp,

        /// <summary>项目已变更</summary>
        Project_Changed,

        /// <summary>项目已加载</summary>
        Project_Loaded,
    }
}