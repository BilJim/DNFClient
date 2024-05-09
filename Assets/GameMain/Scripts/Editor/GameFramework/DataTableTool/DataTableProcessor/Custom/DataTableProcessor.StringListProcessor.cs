using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed partial class DataTableProcessor
{
    /// <summary>
    /// 自定义类的处理器 - ListString
    /// </summary>
    private sealed class StringListTypeProcessor : GenericDataProcessor<List<string>>
    {
        public override bool IsSystem => false;

        public override string LanguageKeyword => "List<string>";

        public override string[] GetTypeStrings()
        {
            return new[]
            {
                "List<string>",
                "System.Collections.Generic.List<string>"
            };
        }

        /// <summary>
        /// 把 string 数据传递到 自定义类实例中
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override List<string> Parse(string value)
        {
            string[] strArr = value.Split(',');
            List<string> resultList = new ();
            resultList.AddRange(strArr);
            return resultList;
        }

        /// <summary>
        /// 把数据写入到二进制流中
        /// 写入的数据是什么样的，读取时就得按照这个顺序。读取代码在 DataTableExtension
        /// </summary>
        public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
        {
            List<string> strList = Parse(value);
            //先写入长度
            binaryWriter.Write(strList.Count);
            foreach (string item in strList)
            {
                binaryWriter.Write(item);
            }
        }
    }
}