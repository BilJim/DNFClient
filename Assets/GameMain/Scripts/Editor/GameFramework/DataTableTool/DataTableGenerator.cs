using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GameFramework;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 通过数据表生成代码的核心逻辑类
/// </summary>
public sealed class DataTableGenerator
{
    //配置表文件所在路径
    private const string DataTablePath = "Assets/GameMain/DataTables";
    //生成的C#代码路径
    private const string CSharpCodePath = "Assets/GameMain/Scripts/DataTable";
    //C#代码模板
    private const string CSharpCodeTemplateFilePath = "Assets/GameMain/Scripts/Editor/GameFramework/DataTableTool/CodeTemplate/DataTableCodeTemplate.txt";
    //C#代码模板
    private const string CSharpCodeTemplateFileWithNamespacePath = "Assets/GameMain/Configs/DataTableTool/CodeTemplate/DataTableCodeTemplate.txt";
    //C#代码文件前缀(此处为DataTable缩写)
    private const string CSharpFilePrefix = "DT";
    //C#代码文件命名空间
    private const string CSharpCodeNamespace = "";
    private static readonly Regex EndWithNumberRegex = new Regex(@"\d+$");
    private static readonly Regex NameRegex = new Regex(@"^[A-Z][A-Za-z0-9_]*$");

    public static DataTableProcessor CreateDataTableProcessor(TextAsset textAsset)
    {
        string relativePath = GetRelativePath(textAsset);
        string dataTableName;
        if (relativePath == null)
            dataTableName = textAsset.name;
        else
            dataTableName = relativePath + textAsset.name;
        
        return new DataTableProcessor(Utility.Path.GetRegularPath(Path.Combine(DataTablePath, dataTableName + ".txt")), Encoding.GetEncoding("GB2312"), 1, 2, null, 3, 4, 1);
    }

    public static bool CheckRawData(DataTableProcessor dataTableProcessor, string dataTableName)
    {
        for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
        {
            string name = dataTableProcessor.GetName(i);
            if (string.IsNullOrEmpty(name) || name == "#")
            {
                continue;
            }

            if (!NameRegex.IsMatch(name))
            {
                Debug.LogWarning(Utility.Text.Format("Check raw data failure. DataTableName='{0}' Name='{1}'", dataTableName, name));
                return false;
            }
        }

        return true;
    }

    public static void GenerateDataFile(DataTableProcessor dataTableProcessor, TextAsset textAsset)
    {
        //相对路径下的文件名
        string relativePathName = GetRelativePath(textAsset);
        if (relativePathName == null)
            return;

        string binaryDataFileName = Utility.Path.GetRegularPath(Path.Combine(DataTablePath, relativePathName + textAsset.name + ".bytes"));
        if (!dataTableProcessor.GenerateDataFile(binaryDataFileName) && File.Exists(binaryDataFileName))
        {
            File.Delete(binaryDataFileName);
        }
    }

    public static void GenerateCodeFile(DataTableProcessor dataTableProcessor, TextAsset textAsset)
    {
        var dataTableName = textAsset.name;
        //相对路径下的文件名
        string relativePathName = GetRelativePath(textAsset);
        if (relativePathName == null)
            return;
        
        if (HasNamespace())
            dataTableProcessor.SetCodeTemplate(CSharpCodeTemplateFileWithNamespacePath, Encoding.UTF8);
        else
            dataTableProcessor.SetCodeTemplate(CSharpCodeTemplateFilePath, Encoding.UTF8);
        dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

        string csharpCodeFileName = Utility.Path.GetRegularPath(Path.Combine(CSharpCodePath, $"{relativePathName}{CSharpFilePrefix}{dataTableName}.cs"));
        //判断文件夹是否存在，若不存在主要要创建文件夹
        string foldPath = Path.Combine(CSharpCodePath, relativePathName);
        if (!Directory.Exists(foldPath))
            Directory.CreateDirectory(foldPath);
        if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, textAsset.name) && File.Exists(csharpCodeFileName))
        {
            File.Delete(csharpCodeFileName);
        }
    }

    /// <summary>
    /// 获取文件相对路径下的文件名
    /// Assets/GameMain/DataTables/UI/UIForm.txt -> "UI/"
    /// Assets/GameMain/DataTables/BaseTable.txt -> ""
    /// </summary>
    /// <returns></returns>
    public static string GetRelativePath(TextAsset textAsset)
    {
        string assetPath = AssetDatabase.GetAssetPath(textAsset);
        if (!assetPath.Contains(DataTablePath))
        {
            Debug.LogError($"当前数据表位置路径不规范: {textAsset.name}");
            return null;
        }
        //相对路径下的文件名
        return assetPath.Replace(DataTablePath + "/", "").Replace($"{textAsset.name}.txt", "");
    }

    private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
    {
        string dataTableName = (string)userData;

        codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
        codeContent.Replace("__DATA_TABLE_NAME_SPACE__", CSharpCodeNamespace);
        codeContent.Replace("__DATA_TABLE_CLASS_NAME__", $"{CSharpFilePrefix}{dataTableName}");
        codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1) + "。");
        codeContent.Replace("__DATA_TABLE_ID_COMMENT__", "获取" + dataTableProcessor.GetComment(dataTableProcessor.IdColumn) + "。");
        codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(dataTableProcessor));
        codeContent.Replace("__DATA_TABLE_PARSER__", GenerateDataTableParser(dataTableProcessor));
        codeContent.Replace("__DATA_TABLE_PROPERTY_ARRAY__", GenerateDataTablePropertyArray(dataTableProcessor));
    }

    private static string GenerateDataTableProperties(DataTableProcessor dataTableProcessor)
    {
        StringBuilder stringBuilder = new StringBuilder();
        bool firstProperty = true;
        
        //有无命名空间影响制表符个数
        int tabSize = HasNamespace() ? 2 : 1;
        for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
        {
            if (dataTableProcessor.IsCommentColumn(i))
            {
                // 注释列
                continue;
            }

            if (dataTableProcessor.IsIdColumn(i))
            {
                // 编号列
                continue;
            }

            if (firstProperty)
            {
                firstProperty = false;
            }
            else
            {
                stringBuilder.AppendLine().AppendLine();
            }

            stringBuilder
                .AppendLine($"{TabsRepeat(tabSize)}/// <summary>")
                .AppendFormat($"{TabsRepeat(tabSize)}/// 获取{dataTableProcessor.GetComment(i)}。").AppendLine()
                .AppendLine($"{TabsRepeat(tabSize)}/// </summary>")
                .AppendFormat($"{TabsRepeat(tabSize)}public {dataTableProcessor.GetLanguageKeyword(i)} {dataTableProcessor.GetName(i)}").AppendLine()
                .Append($"{TabsRepeat(tabSize)}").AppendLine("{")
                .AppendLine($"{TabsRepeat(tabSize+1)}get;")
                .AppendLine($"{TabsRepeat(tabSize+1)}private set;")
                .Append($"{TabsRepeat(tabSize)}").Append("}");
        }

        return stringBuilder.ToString();
    }

    private static string GenerateDataTableParser(DataTableProcessor dataTableProcessor)
    {
        //有无命名空间影响制表符个数
        int tabSize = HasNamespace() ? 2 : 1;
        
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder
            .AppendLine($"{TabsRepeat(tabSize)}public override bool ParseDataRow(string dataRowString, object userData)")
            .Append($"{TabsRepeat(tabSize)}").AppendLine("{")
            .AppendLine(
                $"{TabsRepeat(tabSize+1)}string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);")
            .AppendLine($"{TabsRepeat(tabSize+1)}for (int i = 0; i < columnStrings.Length; i++)")
            .Append($"{TabsRepeat(tabSize+1)}").AppendLine("{")
            .AppendLine(
                $"{TabsRepeat(tabSize+2)}columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);")
            .Append($"{TabsRepeat(tabSize+1)}").AppendLine("}")
            .AppendLine()
            .AppendLine($"{TabsRepeat(tabSize+1)}int index = 0;");
        for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
        {
            if (dataTableProcessor.IsCommentColumn(i))
            {
                // 注释列
                stringBuilder.AppendLine($"{TabsRepeat(tabSize+1)}index++;");
                continue;
            }

            if (dataTableProcessor.IsIdColumn(i))
            {
                // 编号列
                stringBuilder.AppendLine($"{TabsRepeat(tabSize+1)}m_Id = int.Parse(columnStrings[index++]);");
                continue;
            }

            //只有 C# 提供的类才是系统类
            if (dataTableProcessor.IsSystem(i))
            {
                string languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                if (languageKeyword == "string")
                {
                    stringBuilder
                        .AppendFormat($"{TabsRepeat(tabSize+1)}{dataTableProcessor.GetName(i)} = columnStrings[index++];")
                        .AppendLine();
                }
                else
                {
                    stringBuilder.AppendFormat($"{TabsRepeat(tabSize+1)}{dataTableProcessor.GetName(i)} = {languageKeyword}.Parse(columnStrings[index++]);").AppendLine();
                }
            }
            else
            {
                //添加自定义的数据类型
                //List类型
                if (typeof(IList).IsAssignableFrom(dataTableProcessor.GetType(i)))
                {
                    stringBuilder.AppendFormat($"{TabsRepeat(tabSize+1)}{dataTableProcessor.GetName(i)} = DataTableExtension.Parse{dataTableProcessor.GetType(i).GetGenericArguments()[0].Name}List(columnStrings[index++]);").AppendLine();
                }
                else
                {
                    stringBuilder.AppendFormat($"{TabsRepeat(tabSize+1)}{dataTableProcessor.GetName(i)} = DataTableExtension.Parse{dataTableProcessor.GetType(i).Name}(columnStrings[index++]);").AppendLine();
                }
            }
        }

        stringBuilder.AppendLine()
            .AppendLine($"{TabsRepeat(tabSize + 1)}GeneratePropertyArray();")
            .AppendLine($"{TabsRepeat(tabSize + 1)}return true;")
            .Append($"{TabsRepeat(tabSize)}").AppendLine("}")
            .AppendLine()
            .AppendLine(
                $"{TabsRepeat(tabSize)}public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)")
            .Append($"{TabsRepeat(tabSize)}").AppendLine("{")
            .AppendLine(
                $"{TabsRepeat(tabSize + 1)}using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))")
            .Append($"{TabsRepeat(tabSize + 1)}").AppendLine("{")
            .AppendLine(
                $"{TabsRepeat(tabSize + 2)}using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))")
            .Append($"{TabsRepeat(tabSize + 2)}").AppendLine("{");

        for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
        {
            if (dataTableProcessor.IsCommentColumn(i))
                // 注释列
                continue;

            if (dataTableProcessor.IsIdColumn(i))
            {
                // 编号列
                stringBuilder.AppendLine($"{TabsRepeat(tabSize + 3)}m_Id = binaryReader.Read7BitEncodedInt32();");
                continue;
            }

            string languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
            if (languageKeyword == "int" || languageKeyword == "uint" || languageKeyword == "long" ||
                languageKeyword == "ulong")
            {
                stringBuilder.AppendFormat($"{TabsRepeat(tabSize + 3)}{dataTableProcessor.GetName(i)} = binaryReader.Read7BitEncoded{dataTableProcessor.GetType(i).Name}();").AppendLine();
            }
            else
            {
                //添加自定义的数据类型
                //List类型
                if (typeof(IList).IsAssignableFrom(dataTableProcessor.GetType(i)))
                {
                    stringBuilder.AppendFormat($"{TabsRepeat(tabSize + 3)}{dataTableProcessor.GetName(i)} = binaryReader.Read{dataTableProcessor.GetType(i).GetGenericArguments()[0].Name}List();").AppendLine();
                }
                else
                {
                    stringBuilder.AppendFormat($"{TabsRepeat(tabSize + 3)}{dataTableProcessor.GetName(i)} = binaryReader.Read{dataTableProcessor.GetType(i).Name}();").AppendLine();
                }
            }
        }

        stringBuilder
            .Append($"{TabsRepeat(tabSize+2)}").AppendLine("}")
            .Append($"{TabsRepeat(tabSize+1)}").AppendLine("}")
            .AppendLine()
            .AppendLine($"{TabsRepeat(tabSize+1)}GeneratePropertyArray();")
            .AppendLine($"{TabsRepeat(tabSize+1)}return true;")
            .Append($"{TabsRepeat(tabSize)}").AppendLine("}");

        return stringBuilder.ToString();
    }

    private static string GenerateDataTablePropertyArray(DataTableProcessor dataTableProcessor)
    {
        //有无命名空间影响制表符个数
        int tabSize = HasNamespace() ? 2 : 1;
        
        List<PropertyCollection> propertyCollections = new List<PropertyCollection>();
        for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
        {
            if (dataTableProcessor.IsCommentColumn(i))
                // 注释列
                continue;

            if (dataTableProcessor.IsIdColumn(i))
                // 编号列
                continue;

            string name = dataTableProcessor.GetName(i);
            if (!EndWithNumberRegex.IsMatch(name))
                continue;

            string propertyCollectionName = EndWithNumberRegex.Replace(name, string.Empty);
            int id = int.Parse(EndWithNumberRegex.Match(name).Value);

            PropertyCollection propertyCollection = null;
            foreach (PropertyCollection pc in propertyCollections)
            {
                if (pc.Name == propertyCollectionName)
                {
                    propertyCollection = pc;
                    break;
                }
            }

            if (propertyCollection == null)
            {
                propertyCollection =
                    new PropertyCollection(propertyCollectionName, dataTableProcessor.GetLanguageKeyword(i));
                propertyCollections.Add(propertyCollection);
            }

            propertyCollection.AddItem(id, name);
        }

        StringBuilder stringBuilder = new StringBuilder();
        bool firstProperty = true;
        foreach (PropertyCollection propertyCollection in propertyCollections)
        {
            if (firstProperty)
            {
                firstProperty = false;
            }
            else
            {
                stringBuilder.AppendLine().AppendLine();
            }

            stringBuilder
                .AppendFormat($"{TabsRepeat(tabSize)}private KeyValuePair<int, {propertyCollection.LanguageKeyword}>[] m_{propertyCollection.Name} = null;").AppendLine()
                .AppendLine()
                .AppendFormat($"{TabsRepeat(tabSize)}public int {propertyCollection.Name}Count").AppendLine()
                .Append($"{TabsRepeat(tabSize)}").AppendLine("{")
                .AppendLine($"{TabsRepeat(tabSize+1)}get")
                .Append($"{TabsRepeat(tabSize+1)}").AppendLine("{")
                .AppendFormat($"{TabsRepeat(tabSize+2)}return m_{propertyCollection.Name}.Length;").AppendLine()
                .AppendLine($"{TabsRepeat(tabSize+1)}").AppendLine("}")
                .Append($"{TabsRepeat(tabSize)}").AppendLine("}")
                .AppendLine()
                .AppendFormat($"{TabsRepeat(tabSize)}public {propertyCollection.LanguageKeyword} Get{propertyCollection.Name}(int id)").AppendLine()
                .Append($"{TabsRepeat(tabSize)}").AppendLine("{")
                .AppendFormat($"{TabsRepeat(tabSize+1)}foreach (KeyValuePair<int, {propertyCollection.LanguageKeyword}> i in m_{propertyCollection.Name})").AppendLine()
                .Append($"{TabsRepeat(tabSize+1)}").AppendLine("{")
                .AppendLine($"{TabsRepeat(tabSize+2)}if (i.Key == id)")
                .Append($"{TabsRepeat(tabSize+2)}").AppendLine("{")
                .AppendLine($"{TabsRepeat(tabSize+3)}return i.Value;")
                .Append($"{TabsRepeat(tabSize+2)}").AppendLine("}")
                .Append($"{TabsRepeat(tabSize+1)}").AppendLine("}")
                .AppendLine()
                .AppendFormat(
                    $"{TabsRepeat(tabSize+1)}throw new GameFrameworkException(\"Get{propertyCollection.Name} with invalid id {propertyCollection.Name}\");").AppendLine()
                .Append($"{TabsRepeat(tabSize)}").AppendLine("}")
                .AppendLine()
                .AppendFormat($"{TabsRepeat(tabSize)}public {propertyCollection.LanguageKeyword} Get{propertyCollection.Name}At(int index)").AppendLine()
                .Append($"{TabsRepeat(tabSize)}").AppendLine()
                .AppendFormat($"{TabsRepeat(tabSize+1)}if (index < 0 || index >= m_{propertyCollection.Name}.Length)")
                .AppendLine()
                .Append($"{TabsRepeat(tabSize+1)}").AppendLine("{")
                .AppendFormat(
                    $"{TabsRepeat(tabSize+2)}throw new GameFrameworkException(\"Get{propertyCollection.Name}At with invalid index {propertyCollection.Name}.\");").AppendLine()
                .AppendLine($"{TabsRepeat(tabSize+1)}").AppendLine("}")
                .AppendLine()
                .AppendFormat($"{TabsRepeat(tabSize+1)}return m_{propertyCollection.Name}[index].Value;").AppendLine()
                .Append($"{TabsRepeat(tabSize)}").Append("}");
        }

        if (propertyCollections.Count > 0)
        {
            stringBuilder.AppendLine().AppendLine();
        }

        stringBuilder
            .AppendLine($"{TabsRepeat(tabSize)}private void GeneratePropertyArray()")
            .Append($"{TabsRepeat(tabSize)}").AppendLine("{");

        firstProperty = true;
        foreach (PropertyCollection propertyCollection in propertyCollections)
        {
            if (firstProperty)
            {
                firstProperty = false;
            }
            else
            {
                stringBuilder.AppendLine().AppendLine();
            }

            stringBuilder
                .AppendFormat($"{TabsRepeat(tabSize+1)}m_{propertyCollection.Name} = new KeyValuePair<int, {propertyCollection.LanguageKeyword}>[]").AppendLine()
                .Append($"{TabsRepeat(tabSize+1)}").AppendLine("{");

            int itemCount = propertyCollection.ItemCount;
            for (int i = 0; i < itemCount; i++)
            {
                KeyValuePair<int, string> item = propertyCollection.GetItem(i);
                stringBuilder.AppendFormat($"{TabsRepeat(tabSize+2)}new KeyValuePair<int, {propertyCollection.LanguageKeyword}>({item.Key.ToString()}, {item.Value}),").AppendLine();
            }

            stringBuilder.Append($"{TabsRepeat(tabSize+1)}").AppendLine("};");
        }

        stringBuilder
            .AppendLine()
            .Append($"{TabsRepeat(tabSize)}").Append("}");

        return stringBuilder.ToString();
    }

    private sealed class PropertyCollection
    {
        private readonly string m_Name;
        private readonly string m_LanguageKeyword;
        private readonly List<KeyValuePair<int, string>> m_Items;

        public PropertyCollection(string name, string languageKeyword)
        {
            m_Name = name;
            m_LanguageKeyword = languageKeyword;
            m_Items = new List<KeyValuePair<int, string>>();
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public string LanguageKeyword
        {
            get
            {
                return m_LanguageKeyword;
            }
        }

        public int ItemCount
        {
            get
            {
                return m_Items.Count;
            }
        }

        public KeyValuePair<int, string> GetItem(int index)
        {
            if (index < 0 || index >= m_Items.Count)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetItem with invalid index '{index}'."));
            }

            return m_Items[index];
        }

        public void AddItem(int id, string propertyName)
        {
            m_Items.Add(new KeyValuePair<int, string>(id, propertyName));
        }
    }

    /// <summary>
    /// 判断是否有命名空间
    /// </summary>
    private static bool HasNamespace()
    {
        return !string.IsNullOrWhiteSpace(CSharpCodeNamespace);
    }
    
    /// <summary>
    /// 重复制表符
    /// </summary>
    private static string TabsRepeat(int size)
    {
        string tab = "\t";
        StringBuilder sb = new(tab.Length * size);
        for (int i = 0; i < size; i++)
        {
            sb.Append(tab);
        }
        return sb.ToString();
    }
}