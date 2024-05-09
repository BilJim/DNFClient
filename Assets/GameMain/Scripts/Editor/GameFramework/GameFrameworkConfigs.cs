//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.ResourceTools;

/// <summary>
/// 编辑器文件路径相关配置
/// </summary>
public static class GameFrameworkConfigs
{
    /// <summary>
    /// 打包设置生成文件所在目录
    /// </summary>
    [BuildSettingsConfigPath]
    public static string BuildSettingsConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameMain/Scripts/Editor/GameFramework/Configs/BuildSettings.xml"));

    //ResourceEditor 包和资源关联关系配置存储路径
    [ResourceCollectionConfigPath]
    public static string ResourceCollectionConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameMain/Scripts/Editor/GameFramework/Configs/ResourceCollection.xml"));

    //ResourceEditor 配置文件存储路径
    [ResourceEditorConfigPath]
    public static string ResourceEditorConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameMain/Scripts/Editor/GameFramework/Configs/ResourceEditor.xml"));

    [ResourceBuilderConfigPath]
    public static string ResourceBuilderConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameMain/Scripts/Editor/GameFramework/Configs/ResourceBuilder.xml"));
}
