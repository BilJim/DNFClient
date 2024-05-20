using UnityEngine;
using UnityGameFramework.Runtime;

public partial class HeroLogic : TargetableObject
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

        RenderObj = GetComponent<HeroRender>();
        RenderObj.LogicObj = this;
        RenderObj.OnCreate();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        OnUpdateMove(elapseSeconds, realElapseSeconds);
        OnUpdateSkill(elapseSeconds, realElapseSeconds);
        OnUpdateGravity(elapseSeconds, realElapseSeconds);
    }
}