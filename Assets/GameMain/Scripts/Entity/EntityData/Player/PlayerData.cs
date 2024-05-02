using System;
using UnityEngine;

[Serializable]
public class PlayerData : TargetableObjectData
{
    [SerializeField] private string m_Name = null;

    public PlayerData(int entityId, int typeId)
        : base(entityId, typeId, CampType.Player)
    {
    }

    /// <summary>
    /// 角色名称。
    /// </summary>
    public string Name
    {
        get { return m_Name; }
        set { m_Name = value; }
    }

    public override int MaxHP { get; }
}