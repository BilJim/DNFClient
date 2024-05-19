using System;
using UnityEngine;

[Serializable]
public class MonsterData : TargetableObjectData
{

    /// <summary>
    /// 获取碰撞体中心点。
    /// </summary>
    [HideInInspector]
    public Vector3 colliderCenter;

    /// <summary>
    /// 获取碰撞体Size。
    /// </summary>
    [HideInInspector]
    public Vector3 colliderSize;

    public MonsterData(int entityId, int typeId)
        : base(entityId, typeId, CampType.Enemy)
    {
    }

    public override int MaxHP { get; }
}