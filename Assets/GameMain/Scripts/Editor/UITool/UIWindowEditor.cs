using UnityEditor;
using UnityEngine;

/// <summary>
/// 编辑器UI界面 - 代码生成界面
/// </summary>
public class UIWindowEditor : EditorWindow
{
    //生成的脚本代码预览
    private string scriptContent;

    //滚动框
    private Vector2 scroll;

    /// <summary>
    /// 显示代码展示窗口
    /// </summary>
    /// <param name="content">代码文本</param>
    public static void ShowWindow(string content)
    {
        //创建代码展示窗口
        UIWindowEditor window =
            (UIWindowEditor)GetWindowWithRect(typeof(UIWindowEditor), new Rect(100, 50, 800, 700), true, "Window生成界面");
        window.scriptContent = content;
        //弹出窗口
        window.Show();
    }

    /// <summary>
    /// 绘制UI
    /// </summary>
    public void OnGUI()
    {
        //滚动列表
        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(600), GUILayout.Width(800));
        //预览脚本代码
        EditorGUILayout.TextArea(scriptContent);
        EditorGUILayout.EndScrollView();
        //UI间隔
        EditorGUILayout.Space();
    }
}