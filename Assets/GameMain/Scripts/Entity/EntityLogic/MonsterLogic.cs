using FixMath;
using UnityEngine;
using UnityGameFramework.Runtime;

public class MonsterLogic : TargetableObject
{
    [SerializeField] private MonsterData m_MonsterData = null;

    
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_MonsterData = userData as MonsterData;
        if (m_MonsterData == null)
        {
            Log.Error("Monster data is invalid.");
            return;
        }
        SetData();
        RenderObj = GetComponent<MonsterRender>();
        RenderObj.LogicObj = this;
        RenderObj.OnCreate();
        
        BoxColliderGizmo collider = GetComponent<BoxColliderGizmo>();
        collider.SetBoxData(m_MonsterData.colliderCenter, m_MonsterData.colliderSize);
    }

    private void SetData()
    {
        pos = new FixIntVector3(m_MonsterData.bornPosition);
    }
}