using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 生成 UI 元素
/// 方便拷贝到代码中
/// </summary>
public class UIElementCodeGenerate : Editor
{
    
    //查找到的元素数据
    public static List<EditorObjectData> objDataList;
    
    [MenuItem("MyTools/UITools/生成 UI 元素", false, 0)]
    static void CreateCode()
    {
        GameObject obj = Selection.objects.First() as GameObject;//获取到当前选择的物体
        if (obj == null)
        {
            Debug.LogError("需要选择 GameObject");
            return;
        }
        objDataList = new List<EditorObjectData>();
        
        //解析窗口组件数据
        PresWindowNodeData(obj.transform, objDataList);
        
        //生成CS脚本
        string csContnet = CreateCS();
        UIWindowEditor.ShowWindow(csContnet);
    }
    
    /// <summary>
    /// 递归解析窗口节点数据
    /// 通过组件名的方式进行解析例如: [Button]close
    /// </summary>
    /// <param name="trans"></param>
    public static void PresWindowNodeData(Transform trans, List<EditorObjectData> objDataList)
    {
        if (objDataList == null)
            return;
        for (int i = 0; i < trans.childCount; i++)
        {
            GameObject obj = trans.GetChild(i).gameObject;
            string name = obj.name;
            //匹配例如: [Button]Close
            if (Regex.IsMatch(name, UICommon.COMPONENT_PATTERN))
            {
                int index = name.IndexOf("]") + 1;
                //字段名 例: Close
                string fieldName = name.Substring(index, name.Length - index);
                //字段类型 例: Button
                string fieldType = name.Substring(1, index - 2);
                objDataList.Add(new EditorObjectData(obj.GetInstanceID(), fieldName, fieldType));
            }
            PresWindowNodeData(trans.GetChild(i), objDataList);
        }
    }

    public static string CreateCS()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in objDataList)
        {
            sb.AppendLine("[SerializeField]");
            sb.AppendLine($"private {item.fieldType} {item.fieldName};\n");
        }
        return sb.ToString();
    }
}