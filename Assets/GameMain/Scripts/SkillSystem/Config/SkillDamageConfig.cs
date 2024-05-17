using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class SkillDamageConfig
{
    [LabelText("触发帧")] public int triggerFrame;
    [LabelText("结束帧")] public int endFrame;

    [LabelText("触发间隔（毫秒 value=0 默认一次，>0则为间隔）")]
    public int triggerIntervalMs;

    [LabelText("是否跟随特效移动")] public bool isFollowEffect;
    [LabelText("伤害配置")] public DamageType damageType;
    [LabelText("伤害倍率")] public int damageRate;

    [LabelText("伤害检测方式"), OnValueChanged("OnDetectionValueChange")]
    public DamageDetectionMode detectionMode;

    [LabelText("Box碰撞体宽高"), ShowIf("mShowBox3D"), OnValueChanged("OnBoxValueChange")]
    public Vector3 boxSize = new Vector3(1, 1, 1);

    [LabelText("Box碰撞体偏移"), ShowIf("mShowBox3D"), OnValueChanged("OnColliderOffsetChange")]
    public Vector3 boxOffset = new Vector3(0, 0, 0);

    [LabelText("圆球碰撞体偏移值"), ShowIf("mShowSphere3D"), OnValueChanged("OnColliderOffsetChange")]
    public Vector3 sphereOffset = new Vector3(0, 0.9f, 0);

    [LabelText("圆球伤害检测半径"), ShowIf("mShowSphere3D"), OnValueChanged("OnRadiusValueChange")]
    public float radius = 1;

    [LabelText("圆球检测半径高度"), ShowIf("mShowSphere3D")]
    public float radiusHeight = 0;

    [LabelText("碰撞体位置类型")] public ColliderPosType colliderPosType = ColliderPosType.FollowDir;

    [LabelText("伤害触发目标")] public TargetType targetType;

    [TitleGroup("附加Buff", "伤害生效的一瞬间，附加指定的多个buff")]
    public int[] addBuffs;

    [TitleGroup("触发后续技能", "造成伤害后且技能释放完毕后触发的技能")]
    public int triggerSkillId;
}

public enum TargetType
{
    [LabelText("未配置")] None, //未配置
    [LabelText("队友")] Teammate, //队友
    [LabelText("敌人")] Enemy, //敌人
    [LabelText("自身")] Self, //自身
    [LabelText("所有对象")] AllObject, //所有对象
}

public enum ColliderPosType
{
    [LabelText("跟随角色朝向")] FollowDir, //跟随角色朝向
    [LabelText("跟随角色位置")] FollowPos, //跟随角色位置
    [LabelText("中心坐标")] CenterPos, //中心坐标
    [LabelText("目标位置")] TargetPos, //目标位置
}

public enum DamageType
{
    [LabelText("无伤害")] None, //无伤害
    [LabelText("物理伤害")] ADDamage, //物理伤害
    [LabelText("魔法伤害")] APDamage, //魔法伤害
}

public enum DamageDetectionMode
{
    [LabelText("无配置")] None, //无配置
    [LabelText("3DBox碰撞检测")] BOX3D, //3DBox碰撞检测
    [LabelText("3D圆球碰撞检测")] Sphere3D, //3D圆球碰撞检测
    [LabelText("3D圆柱体碰撞检测")] Cylinder3D, //3D圆柱体碰撞检测
    [LabelText("半径的距离")] RadiusDistance, //半径的距离 （代码搜索）
    [LabelText("所有目标")] AllTarget, //通过代码搜索的所有目标
}