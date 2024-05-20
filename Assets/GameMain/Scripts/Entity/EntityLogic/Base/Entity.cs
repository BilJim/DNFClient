using FixIntPhysics;
using FixMath;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体类逻辑基类
/// 注意和框架中 UnityGameFramework.Runtime.Entity 作用不同。框架中 Entity 主要用作于 Entity 管理(分组，生命周期等)
/// 且框架中 Entity 不可被继承
/// </summary>
public abstract class Entity : EntityLogic
{
    [SerializeField] private EntityData m_EntityData = null;

    public int Id => Entity.Id;

    //逻辑位置
    public FixIntVector3 pos;

    //逻辑朝向
    public FixIntVector3 dir;

    //逻辑角度
    public FixIntVector3 angle;

    //逻辑移动速度
    public FixInt moveSpeed;

    //逻辑轴向
    public FixInt axis = 1;

    //逻辑是否激活
    public bool isActive;

    public Animation CachedAnimation { get; private set; }

    /// <summary>
    /// 注意，由于对象池的存在 Entity 会被重复使用，所以尽量不要在这里初始化数据，在 OnShow 中初始化数据
    /// </summary>
    /// <param name="userData"></param>
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        CachedAnimation = GetComponent<Animation>();
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_EntityData = userData as EntityData;
        if (m_EntityData == null)
        {
            Log.Error("Entity data is invalid.");
            return;
        }

        Name = Utility.Text.Format("[Entity {0}]", Id);
        CachedTransform.localPosition = m_EntityData.Position;
        CachedTransform.localRotation = m_EntityData.Rotation;
        CachedTransform.localScale = Vector3.one;
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
    }

    protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
    {
        base.OnAttached(childEntity, parentTransform, userData);
    }

    protected override void OnDetached(EntityLogic childEntity, object userData)
    {
        base.OnDetached(childEntity, userData);
    }

    protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
    {
        base.OnAttachTo(parentEntity, parentTransform, userData);
    }

    protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
    {
        base.OnDetachFrom(parentEntity, userData);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }
}