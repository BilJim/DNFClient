using UnityEngine;
using UnityGameFramework.Runtime;

public class HeroLogic : TargetableObject
{
    [SerializeField] private HeroData m_HeroData = null;

    
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_HeroData = userData as HeroData;
        if (m_HeroData == null)
        {
            Log.Error("Hero data is invalid.");
            return;
        }
    }
}