using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class SkillEffectConfig
{
    [AssetList] [LabelText("技能特效对象")] [PreviewField(70, ObjectFieldAlignment.Left), OnValueChanged("GetObjectPath")]
    public GameObject skillEffect;

    [ReadOnly] public string skillEffectPath;
    [LabelText("触发帧")] public int triggerFrame;
    [LabelText("结束帧")] public int endFrame;
    [LabelText("特效偏移位置")] public Vector3 effectOffsetPos;
    [LabelText("特效位置类型")] public EffectPosType effectPosType;

    [ToggleGroup("isSetTransParent", "是否设置特效父节点")]
    public bool isSetTransParent = false;

    [ToggleGroup("isSetTransParent", "节点类型")]
    public TransParentType transParent;

    [ToggleGroup("isAttachDamage", "是否附加伤害")]
    public bool isAttachDamage = false;

    [ToggleGroup("isAttachDamage", "是否附加伤害")]
    public SkillDamageConfig damageConfig;

    [ToggleGroup("isAttachAction", "是否附加行动")]
    public bool isAttachAction = false;

    [ToggleGroup("isAttachAction", "是否附加行动")]
    public SkillActionConfig actionConfig;
}

public enum TransParentType
{
    [LabelText("无配置")] None,
    [LabelText("左手")] LeftHand,
    [LabelText("右手")] RightHand,
}

/// <summary>
/// 特效位置类型
/// </summary>
public enum EffectPosType
{
    [LabelText("跟随角色位置和方向")] FollowPosDir,
    [LabelText("跟随角色方向")] FollowDir,
    [LabelText("屏幕中心位置")] CenterPos,
    [LabelText("引导位置")] GuidePos,
    [LabelText("跟随特效移动位置")] FollowEffectMovePos,
    [LabelText("位置归零")] Zero,
}