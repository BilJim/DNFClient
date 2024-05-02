using System;
using UnityEngine;

/// <summary>
/// 可被攻击的实体 - Data
/// </summary>
[Serializable]
public abstract class TargetableObjectData : EntityData
{
    
    [SerializeField] private CampType m_Camp = CampType.Unknown;
    
    [SerializeField] private int m_HP = 0;

    public TargetableObjectData(int entityId, int typeId, CampType camp)
        : base(entityId, typeId)
    {
        m_Camp = camp;
    }

    /// <summary>
    /// 角色阵营。
    /// </summary>
    public CampType Camp => m_Camp;

    /// <summary>
    /// 当前生命。
    /// </summary>
    public int HP
    {
        get { return m_HP; }
        set { m_HP = value; }
    }

    /// <summary>
    /// 最大生命。
    /// </summary>
    public abstract int MaxHP { get; }

    /// <summary>
    /// 生命百分比。
    /// </summary>
    public float HPRatio => MaxHP > 0 ? (float)HP / MaxHP : 0f;
}