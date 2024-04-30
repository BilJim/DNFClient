using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 绑定 UI 元素
/// </summary>
public class UIElementBind : Editor
{
    //查找到的元素数据
    public static List<EditorObjectData> objDataList;

    [MenuItem("MyTools/UITools/绑定 UI 元素", false, 1)]
    static void BindElement()
    {
        //获取到当前选择的物体
        GameObject obj = Selection.objects.First() as GameObject;
        //针对于 GF 框架下的 UI 组件
        UGuiForm uiForm = obj?.GetComponent<UGuiForm>();
        if (uiForm == null)
            return;
        objDataList = new List<EditorObjectData>();
        //遍历 UI组件元素
        UIElementCodeGenerate.PresWindowNodeData(obj.transform, objDataList);
        for (var i = 0; i < objDataList.Count; i++)
        {
            EditorObjectData item = objDataList[i];
            FieldInfo fieldItem =
                uiForm.GetType().GetField(item.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldItem == null)
                continue;
            //根据Insid找到对应的对象
            GameObject uiObject = EditorUtility.InstanceIDToObject(item.instId) as GameObject;
            fieldItem.SetValue(uiForm, uiObject.GetComponent(item.fieldType));
        }
    }
}