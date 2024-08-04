namespace XLib.Base
{
    /// <summary>
    /// 数据窗口
    /// </summary>
    public class DataWindow<T>
    {
        /// <summary>数据</summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>窗口高度</summary>
        public int WindowHeight { get => _windowHeight; set => _windowHeight = value; }

        /// <summary>数据量</summary>
        public int DataCount => Data.Count;

        public int Top { get => _top; set => _top = value; }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            Data.Clear();
            _startIndex = 0;
            _top = 0;
        }

        /// <summary>
        /// 获取可见数据
        /// </summary>
        public List<T> GetVisibleData()
        {
            // 数据源全部可见
            if (Data.Count <= _windowHeight) return Data;

            List<T> result = new List<T>();

            // 尾端索引 = 起始索引 + 窗口大小
            int endIndex = _startIndex + _windowHeight;
            // 尾端空白数
            int endSpace = endIndex - Data.Count;
            // 前移起始索引
            if (endSpace > 0)
            {
                _startIndex -= endSpace;
                if (_startIndex < 0) _startIndex = 0;
            }
            // 插入可见数据
            for (int counter = 0; counter < WindowHeight; counter++)
            {
                result.Add(Data[_startIndex + counter]);
            }

            return result;
        }

        /// <summary>
        /// 获取可视数据
        /// </summary>
        /// <param name="residue">是否有剩余空间</param>
        public List<T> GetVisibleDataNew(bool residue = false)
        {
            if (Data.Count <= WindowHeight) return new List<T>(Data);

            List<T> result = new List<T>();
            int endIndex = _top + _windowHeight;
            if (residue) endIndex++;
            for (int index = _top; index < endIndex; index++)
            {
                if (index > Data.Count - 1) break;
                result.Add(Data[index]);
            }
            return result;
        }

        #region 属性字段

        private int _windowHeight = 10;
        private int _startIndex;
        private int _top = 0;

        #endregion
    }
}