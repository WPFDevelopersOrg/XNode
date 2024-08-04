using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace XLib.Base.Ex
{
    public static class StringEx
    {
        public static string TimeFormat3 { get; set; } = @"mm\:ss\.fff";

        public static string TimeFormat4 { get; set; } = @"hh\:mm\:ss\.fff";

        public static string ShortTime { get; set; } = @"mm\:ss";

        /// <summary>
        /// 转换为整型列表
        /// </summary>
        public static List<int> ToIntList(this string source, char splitChar = ',')
        {
            try
            {
                List<int> result = new List<int>();
                if (source == "") return result;

                source = source.Trim();
                if (splitChar != ' ') source = source.Replace(" ", "");
                foreach (var item in source.Split(splitChar))
                    result.Add(int.Parse(item));
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("转换为整型列表发生异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 转换为字节列表
        /// </summary>
        public static List<byte> ToByteList(this string source, char splitChar = ',')
        {
            try
            {
                List<byte> result = new List<byte>();
                if (source == "") return result;
                source = source.Trim();
                if (splitChar != ' ') source = source.Replace(" ", "");
                foreach (var item in source.Split(splitChar))
                    result.Add(byte.Parse(item));
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("转换为字节列表发生异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 转换为字符串列表
        /// </summary>
        public static List<string> ToStringList(this string source, char splitChar = ',')
        {
            try
            {
                List<string> result = new List<string>();
                if (source == "") return result;
                source = source.Trim();
                if (splitChar != ' ') source = source.Replace(" ", "");
                foreach (var item in source.Split(splitChar))
                    result.Add(item);
                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("转换为字符串列表发生异常：" + ex.Message);
                return new List<string>();
            }
        }

        /// <summary>
        /// 转换为二进制
        /// </summary>
        public static string ToBinary(this string source)
        {
            // 转换为字节数组
            byte[] byteArray = Encoding.UTF8.GetBytes(source);
            // 转换为二进制字符串
            StringBuilder builder = new StringBuilder(byteArray.Length * 8);
            foreach (var item in byteArray)
                builder.Append(Convert.ToString(item, 2).PadLeft(8, '0'));
            return builder.ToString();
        }

        /// <summary>
        /// 转字节数组
        /// </summary>
        public static string ToByteArray(this string source)
        {
            // 转换为字节数组
            byte[] byteArray = Encoding.UTF8.GetBytes(source);
            StringBuilder builder = new StringBuilder();
            foreach (var item in byteArray)
                builder.Append(item).Append(',');
            return builder.ToString()[..^1];
        }

        /// <summary>
        /// 将字节数组形式的字符串转换为普通字符串
        /// </summary>
        public static string FromByteArray(this string source)
        {
            string[] strArray = source.Split(',');
            byte[] byteArray = new byte[strArray.Length];
            for (int index = 0; index < strArray.Length; index++)
                byteArray[index] = byte.Parse(strArray[index]);
            return Encoding.UTF8.GetString(byteArray);
        }

        /// <summary>
        /// 转换为二进制并压缩
        /// </summary>
        public static string BinaryCompress(this string source)
        {
            return "";
        }

        /// <summary>
        /// 解压缩二进制字符串
        /// </summary>
        public static string BinaryUnCompress(this string source)
        {
            return "";
        }

        /// <summary>
        /// 是否为有效网络地址
        /// </summary>
        public static bool IsIP(this string source) => IPAddress.TryParse(source, out _);

        /// <summary>
        /// 移除扩展名
        /// </summary>
        public static string RemoveExtension(this string source)
        {
            // 无扩展名，直接返回
            if (!source.Contains('.')) return source;
            // 分割
            string[] parts = source.Split(".");
            string result = "";
            // 拼接（忽略最后一块）
            for (int counter = 0; counter < parts.Length - 1; counter++)
                result += parts[counter] + ".";
            // 移除最后一个点
            return result[..^1];
        }

        /// <summary>
        /// 重命名路径
        /// </summary>
        public static string RenamePath(this string path, string split, string newName)
        {
            if (path != "" && split != "" && newName != "")
            {
                TreeNodePath nodePath = new TreeNodePath(path, split);
                if (nodePath.NodeList.Count > 0)
                    nodePath.NodeList[^1] = newName;
                return nodePath.ToString();
            }
            throw new Exception("重命名路径失败：参数不能为空");
        }

        /// <summary>
        /// 封装为列表
        /// </summary>
        public static List<string> PackToList(this string source) => new List<string> { source };

        /// <summary>
        /// 解析路径
        /// </summary>
        public static (string, string) ParsePath(this string source, string split)
        {
            string path;
            string name;

            TreeNodePath nodePath = new TreeNodePath(source, split);
            name = nodePath.NodeList[^1];
            nodePath.RemoveLast();
            path = nodePath.ToString();

            return (path, name);
        }

        /// <summary>
        /// 移除路径的结尾分割符
        /// </summary>
        public static string RemoveTailSplit(this string path)
        {
            if (path.EndsWith("\\")) return path[..^1];
            return path;
        }

        /// <summary>
        /// 解析为路径
        /// </summary>
        public static List<string> ParseToPath(this string path, string split = "/")
        {
            if (path != "")
            {
                // 移除前后空格
                path = path.Trim();
                // 移除前后路径分割符
                if (path.StartsWith(split)) path = path[1..];
                if (path.EndsWith(split)) path = path[..^1];
                // 分割路径
                if (path.Contains(split)) return path.Split(split).ToList();
                return new List<string> { path };
            }
            return new List<string>();
        }

        /// <summary>
        /// 将时间字符串转为毫秒
        /// </summary>
        public static int ToMs(this string time)
        {
            if (time == "") return 0;
            if (IsTimeString(time))
                return (int)TimeSpan.ParseExact(time, TimeFormat4, CultureInfo.InvariantCulture).TotalMilliseconds;
            return (int)TimeSpan.ParseExact(time, TimeFormat3, CultureInfo.InvariantCulture).TotalMilliseconds;
        }

        /// <summary>
        /// 将时间字符串转为秒
        /// </summary>
        public static int ToSecond(this string time)
        {
            return (int)TimeSpan.ParseExact(time, ShortTime, CultureInfo.InvariantCulture).TotalSeconds;
        }

        /// <summary>
        /// 是否为时间字符串。格式：00:00:00.000
        /// </summary>
        public static bool IsTimeString(this string time)
        {
            string pattern = @"^([01]?\d|2[0-3]):([0-5]\d):([0-5]\d)\.(\d{1,3})$";
            return Regex.Match(time, pattern).Success;
        }
    }
}