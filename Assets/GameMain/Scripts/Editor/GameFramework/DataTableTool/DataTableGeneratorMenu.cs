using UnityEditor;
using UnityEngine;

public class DataTableGeneratorMenu
{
    [MenuItem("Game Framework/生成数据表Code和二进制文件(数据表专用)", false, 100)]
    private static void GenerateDataTables()
    {
        //暂未实现对文件的筛选
        Object[] files = Selection.objects;
        if (files == null || files.Length == 0)
            return;
        foreach (Object item in files)
        {
            //过滤非文本类型的资源
            if (item.GetType() != typeof(TextAsset))
                continue;
            DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(item as TextAsset);
            if (!DataTableGenerator.CheckRawData(dataTableProcessor, item.name))
            {
                Debug.LogError($"检查原始数据失败. DataTableName: {item.name}");
                break;
            }
            
            DataTableGenerator.GenerateDataFile(dataTableProcessor, item as TextAsset);
            DataTableGenerator.GenerateCodeFile(dataTableProcessor, item as TextAsset);
        }
        AssetDatabase.Refresh();
    }
}