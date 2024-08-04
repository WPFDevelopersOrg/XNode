namespace XLib.Base.Ex
{
    public static class ListExtension
    {
        /// <summary>
        /// 判断索引是否越界
        /// </summary>
        public static bool IndexOut<T>(this List<T> list, int index)
        {
            return index < 0 || index > list.Count - 1;
        }

        /// <summary>
        /// 移除最后一个元素
        /// </summary>
        public static void RemoveLast<T>(this List<T> list) => list.RemoveAt(list.Count - 1);

        /// <summary>
        /// 追加元素
        /// </summary>
        public static List<T> AppendElement<T>(this List<T> sourceList, T element)
        {
            sourceList.Add(element);
            return sourceList;
        }

        /// <summary>
        /// 列表转字符串
        /// </summary>
        public static string ToListString<T>(this List<T> list, string split = " ")
        {
            if (list.Count == 0) return "";

            string result = "";
            foreach (var item in list) result += $"{item}{split}";
            return result[..^split.Length];
        }

        /// <summary>
        /// 封装为列表
        /// </summary>
        public static List<T> PackToList<T>(this T item) => new List<T> { item };

        /// <summary>
        /// 复制元素至
        /// </summary>
        public static void CopyTo<T>(this List<T> source, List<T> target)
        {
            int copyLength = Math.Min(source.Count, target.Count);
            for (int index = 0; index < copyLength; index++)
                target[index] = source[index];
        }

        /// <summary>
        /// 偏移列表
        /// </summary>
        public static List<T> Offset<T>(this List<T> source, int offset)
        {
            List<T> result = new List<T>();
            offset %= source.Count;
            if (offset < 0) offset = source.Count + offset;
            for (int index = offset; index < source.Count; index++)
                result.Add(source[index]);
            for (int index = 0; index < offset; index++)
                result.Add(source[index]);
            return result;
        }

        /// <summary>
        /// 反相
        /// </summary>
        public static void Invert(this List<byte> source)
        {
            for (int index = 0; index < source.Count; index++)
                source[index] = (byte)(255 - source[index]);
        }

        /// <summary>
        /// 随机元素顺序
        /// </summary>
        public static List<int> RandomElementOrder(this List<int> source)
        {
            List<int> result = new List<int>();

            Random random = new Random();
            int index;
            int count = source.Count;
            while (count > 0)
            {
                // 随机索引
                index = random.Next(source.Count);
                // 添加元素至结果列表
                result.Add(source[index]);
                // 从源列表移除元素
                source.RemoveAt(index);
                // 更新计数
                count--;
            }

            return result;
        }
    }
}