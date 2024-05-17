using FixIntPhysics;
using UnityEngine;

public partial class SkillDamageConfig
{
#if UNITY_EDITOR
    //是否显示 3DBox 碰撞体
    private bool mShowBox3D;

    //是否显示 3D圆球 碰撞体
    private bool mShowSphere3D;
    private FixIntBoxCollider boxCollider;
    private FixIntSphereCollider sphereCollider;
    private int mCurLogicFrame = 0;

    /// <summary>
    /// 碰撞检测类型发生变化
    /// </summary>
    /// <param name="detectionMode"></param>
    public void OnDetectionValueChange(DamageDetectionMode detectionMode)
    {
        mShowBox3D = detectionMode == DamageDetectionMode.BOX3D;
        mShowSphere3D = detectionMode == DamageDetectionMode.Sphere3D;
        CreateCollider();
    }

    /// <summary>
    /// 圆球碰撞体检测半径发生变化
    /// </summary>
    /// <param name="radius"></param>
    public void OnRadiusValueChange(float radius)
    {
        sphereCollider?.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
    }

    /// <summary>
    /// 碰撞体中心点发生变化
    /// </summary>
    public void OnColliderOffsetChange(Vector3 center)
    {
        if (detectionMode == DamageDetectionMode.BOX3D && boxCollider != null)
            boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
        else if (detectionMode == DamageDetectionMode.Sphere3D && sphereCollider != null)
            sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
    }

    /// <summary>
    /// Box碰撞体宽高发生变化
    /// </summary>
    public void OnBoxValueChange(Vector3 size)
    {
        boxCollider?.SetBoxData(GetColliderOffsetPos(), size, colliderPosType == ColliderPosType.FollowPos);
    }

    /// <summary>
    /// 获取碰撞体的偏移值
    /// </summary>
    /// <returns></returns>
    public Vector3 GetColliderOffsetPos()
    {
        Vector3 characterPos = SkillComplierWindow.GetCharacterPos();
        if (detectionMode == DamageDetectionMode.BOX3D)
            return characterPos + boxOffset;
        if (detectionMode == DamageDetectionMode.Sphere3D)
            return characterPos + sphereOffset;
        return Vector3.zero;
    }

    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public void CreateCollider()
    {
        DestroyCollider();
        if (detectionMode == DamageDetectionMode.BOX3D)
        {
            boxCollider = new FixIntBoxCollider(boxSize, GetColliderOffsetPos());
            boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
        }
        else if (detectionMode == DamageDetectionMode.Sphere3D)
        {
            sphereCollider = new FixIntSphereCollider(radius, GetColliderOffsetPos());
            sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        }
    }

    public void DestroyCollider()
    {
        boxCollider?.OnRelease();
        sphereCollider?.OnRelease();
    }

    /// <summary>
    /// 当前窗口初始化
    /// </summary>
    public void OnInit()
    {
        CreateCollider();
    }

    /// <summary>
    /// 当前窗口关闭
    /// </summary>
    public void OnRelease()
    {
        DestroyCollider();
    }

    public void PlaySkillStart()
    {
        mCurLogicFrame = 0;
        DestroyCollider();
    }

    public void PlaySkillEnd()
    {
        DestroyCollider();
    }

    public void OnLogicFrameUpdate()
    {
        //是否到达触发帧
        if (mCurLogicFrame == triggerFrame)
            CreateCollider();
        else if (mCurLogicFrame == endFrame)
            DestroyCollider();

        mCurLogicFrame++;
    }
#endif
}