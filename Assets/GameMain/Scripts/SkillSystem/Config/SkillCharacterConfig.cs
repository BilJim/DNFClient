using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 技能所属人物配置
/// </summary>
[HideMonoScript] //该特性用于隐藏窗口中当前类的信息
[Serializable]
public partial class SkillCharacterConfig
{
    [AssetList] [LabelText("角色模型")] [PreviewField(70, ObjectFieldAlignment.Center)]
    public GameObject skillCharacter;

    [TitleGroup("技能渲染", "英雄渲染数据会在技能开始释放时触发")] [LabelText("技能动画")]
    public AnimationClip skillAnim;

    [BoxGroup("动画数据")] [ProgressBar(0, 100, r: 0, g: 125, b: 125, Height = 30)] [HideLabel]
    public short animProgress;

    [BoxGroup("动画数据")] [LabelText("是否是循环动画")]
    public bool isLoopAnim;

    [BoxGroup("动画数据")] [LabelText("循环次数(动画时长 = 单次时间 * 循环次数)")] [ShowIf("isLoopAnim")]
    public int loopTimes = 1;

    [BoxGroup("动画数据")] [LabelText("逻辑帧(动画时长/0.066, 每秒15帧为前提计算)")]
    public int logicFrame;

    [BoxGroup("动画数据")] [LabelText("动画时长")] public float animLength;

    [FormerlySerializedAs("skillDurationMS")]
    [FormerlySerializedAs("skillDuration")]
    [BoxGroup("动画数据")]
    [LabelText("技能推荐时长(毫秒ms)")]
    public float skillDurationMs = 0;
}