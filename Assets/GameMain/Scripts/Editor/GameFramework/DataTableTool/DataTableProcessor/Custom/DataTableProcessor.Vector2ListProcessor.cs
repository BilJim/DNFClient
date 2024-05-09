using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed partial class DataTableProcessor
{
    /// <summary>
    /// 自定义类的处理器 - ListVector2
    /// </summary>
    private sealed class Vector2ListTypeProcessor : GenericDataProcessor<List<Vector2>>
    {
        public override bool IsSystem
        {
            get { return false; }
        }

        public override string LanguageKeyword
        {
            get { return "List<Vector2>"; }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "List<Vector2>",
                "System.Collections.Generic.List<Vector2>"
            };
        }

        /// <summary>
        /// 把 Vector2 数据传递到 自定义类实例中
        /// </summary>
        /// <param name="value">(6.6,-2.35);(4.78,-3.5);(0.79,-4.73);(-2.53,-3.04)</param>
        /// <returns></returns>
        public override List<Vector2> Parse(string value)
        {
            string[] splitedValue = value.Split(';');
            List<Vector2> resultList = new();
            for (int index = 0; index < splitedValue.Length; index++)
            {
                var str = splitedValue[index];
                //移除 (
                str = str.Remove(0, 1);
                //移除 )
                str = str.Remove(str.Length - 1, 1);
                string[] vectorStr = str.Split(",");
                resultList.Add(new Vector2(float.Parse(vectorStr[0]), float.Parse(vectorStr[1])));
            }

            return resultList;
        }

        /// <summary>
        /// 把数据写入到二进制流中
        /// 写入的数据是什么样的，二进制读取时就得按照这个顺序。读取代码在 DataTableExtension
        /// </summary>
        public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter,
            string value)
        {
            List<Vector2> resultList = Parse(value);
            //先写入长度
            binaryWriter.Write(resultList.Count);
            foreach (Vector2 item in resultList)
            {
                binaryWriter.Write(item.x);
                binaryWriter.Write(item.y);
            }
        }
    }
}