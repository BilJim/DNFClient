using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 用于移除预制体上失效的脚本
/// 使用方法：选中预制体后(支持多选)，在菜单栏 GameObject 下点击 移除预制体上失效的脚本
/// </summary>
public class RemoveMissingScripts : EditorWindow
{
    static int m_goCount;
    static int m_missingCount;

    [MenuItem("GameObject/移除预制体上失效的脚本")]
    static void Apply()
    {
        var beginTime = DateTime.UtcNow.Ticks;

        m_goCount = 0;
        m_missingCount = 0;
        foreach (var go in Selection.gameObjects)
            search(go);

        var endTime = DateTime.UtcNow.Ticks;
        var deltaTime = endTime - beginTime;

        Debug.Log(
            $"Searched in {m_goCount} GameObjects, found and removed {m_missingCount} missing scripts. Took {deltaTime / 10000.0} ms.");
    }

    static void search(GameObject go)
    {
        m_goCount++;
        m_missingCount += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        foreach (Transform child in go.transform)
            search(child.gameObject);
    }
}