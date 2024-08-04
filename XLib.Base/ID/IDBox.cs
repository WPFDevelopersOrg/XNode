using XLib.Base.Ex;

namespace XLib.Base.ID
{
    /// <summary>
    /// 编号箱子
    /// </summary>
    public class IDBox
    {
        #region 属性

        /// <summary>错误信息</summary>
        public string ErrorInfo { get; set; } = "";

        #endregion

        #region 公开方法

        /// <summary>
        /// 取出编号
        /// </summary>
        public int TakeID()
        {
            // 获取一个未使用的格子
            IDGrid? grid = GetUnusedGrid();
            if (grid == null) return -1;
            // 取出编号
            grid.Used = true;
            _usedIDList.Add(grid.ID);
            return grid.ID;
        }

        /// <summary>
        /// 使用编号
        /// </summary>
        public void UseID(int id)
        {
            if (id <= _gridList.Count) _gridList[id - 1].Used = true;
            if (!_usedIDList.Contains(id)) _usedIDList.Add(id);
        }

        /// <summary>
        /// 回收编号
        /// </summary>
        public void RecycleID(int id)
        {
            // 设置为未使用
            if (!_gridList.IndexOut(id - 1))
                _gridList[id - 1].Used = false;
            // 移除已使用的编号
            _usedIDList.Remove(id);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _gridList.Clear();
            _usedIDList.Clear();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取一个未使用的格子
        /// </summary>
        private IDGrid? GetUnusedGrid()
        {
            while (true)
            {
                // 查找可用格子
                foreach (var grid in _gridList)
                    if (!grid.Used) return grid;

                try { GenerateGrid(); }
                catch (Exception ex)
                {
                    ErrorInfo = ex.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// 生成新的格子，并返回生成的第一个格子
        /// </summary>
        private void GenerateGrid()
        {
            int startIndex = _gridList.Count;
            for (int counter = 0; counter < 64; counter++)
            {
                // 创建并添加格子
                IDGrid grid = new IDGrid { ID = startIndex + counter + 1 };
                _gridList.Add(grid);
                // 如果格子的编号已使用
                if (_usedIDList.Contains(grid.ID)) grid.Used = true;
            }
        }

        #endregion

        #region 字段

        /// <summary>格子列表</summary>
        private readonly List<IDGrid> _gridList = new List<IDGrid>();
        /// <summary>已使用的编号列表</summary>
        private readonly List<int> _usedIDList = new List<int>();

        #endregion
    }
}