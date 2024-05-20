using UnityEngine;

/// <summary>
/// Entity 渲染层
/// </summary>
public abstract class EntityRender : MonoBehaviour
{
    //逻辑对象
    private Entity m_LogicObj;
    public Entity LogicObj
    {
        get => m_LogicObj;
        set
        {
            m_LogicObj = value;
            transform.position = value.pos.ToVector3();
        }
    }

    //位置插值移动速度
    protected float mSmoothPosSpeed;

    protected void UpdatePosition(float elapseSeconds, float realElapseSeconds)
    {
        transform.position = Vector3.Lerp(transform.position, LogicObj.pos.ToVector3(), elapseSeconds);
    }

    //更新朝向
    protected void UpdateDir(float elapseSeconds, float realElapseSeconds)
    {
        transform.rotation = Quaternion.Euler(LogicObj.dir.ToVector3());
    }

    public abstract void OnCreate();

    public abstract void OnRelease();
}