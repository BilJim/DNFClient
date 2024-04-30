using System;
using System.IO;

/// <summary>
/// 数据表处理器
/// </summary>
public sealed partial class DataTableProcessor
{
    public abstract class DataProcessor
    {
        /// <summary>
        /// 返回数据类型
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// 是否是ID
        /// </summary>
        public abstract bool IsId { get; }

        /// <summary>
        /// 是否是ID字段
        /// </summary>
        public abstract bool IsComment { get; }

        /// <summary>
        /// 是否是C#系统类
        /// </summary>
        public abstract bool IsSystem { get; }

        /// <summary>
        /// 数据类型（字符串）
        /// </summary>
        public abstract string LanguageKeyword { get; }

        /// <summary>
        /// 对应 Excel 中的数据类型，至于为什么是数组
        /// 因为可以匹配类型别名的情况
        /// 比如 string & System.String 都对应了同一种数据类型
        /// </summary>
        public abstract string[] GetTypeStrings();

        /// <summary>
        /// 写入二进制流的规则
        /// </summary>
        /// <param name="dataTableProcessor"></param>
        /// <param name="binaryWriter"></param>
        /// <param name="value"></param>
        public abstract void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter,
            string value);
    }
}