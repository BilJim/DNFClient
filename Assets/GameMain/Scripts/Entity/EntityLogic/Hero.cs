using UnityEngine;
using UnityGameFramework.Runtime;

public class Hero : TargetableObject
{
    [SerializeField] private HeroData m_HeroData = null;

    
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_HeroData.Name = GameEntry.DataNode.GetData<VarString>("Player.Name");
    }
}