using System;
using UnityEngine;

[Serializable]
public class HeroData : TargetableObjectData
{
    [SerializeField] private string m_Name = null;

    public HeroData(int entityId, int typeId)
        : base(entityId, typeId, CampType.Player)
    {
    }

    /// <summary>
    /// 自定义玩家名
    /// </summary>
    public string Name
    {
        get { return m_Name; }
        set { m_Name = value; }
    }

    public override int MaxHP { get; }
}