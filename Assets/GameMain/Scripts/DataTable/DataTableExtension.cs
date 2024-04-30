using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 通过 txt 读取 DataTables 数据
/// </summary>
public static class DataTableExtension
{
    //可自定义，例 FightingGame.DATA | FightingGame是这个类的命名空间，DATA是这个类自动加的前缀
    //如果没有命名空间，'.' 也必须存在
    private const string DataRowClassPrefixName = ".DT";
    internal static readonly char[] DataSplitSeparators = new char[] { '\t' };
    internal static readonly char[] DataTrimSeparators = new char[] { '\"' };

    public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName,
        string dataTableAssetName, object userData)
    {
        if (string.IsNullOrEmpty(dataTableName))
        {
            Log.Warning("Data table name is invalid.");
            return;
        }

        //不能有下划线 _
        string[] splitedNames = dataTableName.Split('_');
        if (splitedNames.Length > 2)
        {
            Log.Warning("Data table name is invalid.");
            return;
        }

        
        string dataRowClassName;
        //针对有父文件夹的数据表的处理，例如: UI/UIForm
        int splitIndex = splitedNames[0].LastIndexOf("/");
        splitIndex = splitIndex < 0 ? 0 : splitIndex + 1;
        string originTableName = splitedNames[0].Substring(splitIndex);
        //针对于命名空间的处理
        string[] splitStr = DataRowClassPrefixName.Split(".");
        if (string.IsNullOrWhiteSpace(splitStr[0]))
            dataRowClassName = splitStr[1] + originTableName;
        else
            dataRowClassName = DataRowClassPrefixName + originTableName;
        
        Type dataRowType = Type.GetType(dataRowClassName);
        if (dataRowType == null)
        {
            Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
            return;
        }

        string name = splitedNames.Length > 1 ? splitedNames[1] : null;
        DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
        dataTable.ReadData(dataTableAssetName, 100, userData);
    }

    public static Color32 ParseColor32(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Color32(byte.Parse(splitedValue[0]), byte.Parse(splitedValue[1]), byte.Parse(splitedValue[2]),
            byte.Parse(splitedValue[3]));
    }

    public static Color ParseColor(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    public static Quaternion ParseQuaternion(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    public static Rect ParseRect(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Rect(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    public static Vector2 ParseVector2(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
    }

    public static Vector3 ParseVector3(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
    }

    public static Vector4 ParseVector4(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector4(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]),
            float.Parse(splitedValue[3]));
    }

    /// <summary>
    /// 自定义类的读取处理
    /// </summary>
    public static List<Vector2> ParseVector2List(string value)
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
    public static List<string> ParseStringList(string value)
    {
        string[] splitedValue = value.Split(',');
        List<string> resultList = new(splitedValue);
        return resultList;
    }
}