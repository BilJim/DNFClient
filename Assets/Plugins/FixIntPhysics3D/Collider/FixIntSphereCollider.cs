using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FixIntPhysics
{
    public class FixIntSphereCollider : ColliderBehaviour
    {
        /// <summary>
        /// 球形检测范围配置
        /// </summary>
        public SphereColliderGizmo mSphereGizmoObj;
        /// <summary>
        /// 检测半径
        /// </summary>
        public FixInt Radius { get; private set; }
        /// <summary>
        /// 是否跟随目标 若跟随，球形碰撞范围则持续跟随
        /// </summary>
        private bool mIsFollowTarget;
        public FixIntSphereCollider(FixInt radius, Vector3 center)
        {
            this.Radius = radius;
            this.ColliderType = ColliderType.Shpere;
            this.Conter = new FixIntVector3(center);
        }
        public FixIntSphereCollider(FixInt radius, FixIntVector3 center)
        {
            this.Radius = radius;
            this.ColliderType = ColliderType.Shpere;
            this.Conter = center;
        }
        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(FixIntVector3 pos, FixIntVector3 size = default, FixInt radius = default)
        {
            base.UpdateColliderInfo(pos, size, radius);
            this.Radius = radius / 2;
#if UNITY_EDITOR
            this.mSphereGizmoObj.transform.position = pos.ToVector3();
#endif
        }
        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(Vector3 pos, Vector3 size = default, float radius = default)
        {
            this.LogicPosition = new FixIntVector3(pos);
            this.Radius = new FixInt(radius) / 2;
#if UNITY_EDITOR
            this.mSphereGizmoObj.transform.position = pos;
#endif
        }
        /// <summary>
        /// 设置碰撞信息
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="center">中心偏移位置</param>
        /// <param name="isFollowTarget">是否跟随目标</param>
        public override void SetBoxData(float radius, Vector3 center, bool isFollowTarget = false)
        {
            SetBoxData(radius,new FixIntVector3(center),isFollowTarget);
        }

        public override void SetBoxData(FixInt radius, FixIntVector3 center, bool isFollowTarget = false)
        {
#if UNITY_EDITOR
            if (mSphereGizmoObj == null)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mSphereGizmoObj = obj.AddComponent<SphereColliderGizmo>();
            }
#endif
            mIsFollowTarget = isFollowTarget;
            this.Conter = center;
            this.Radius = radius;
#if UNITY_EDITOR
            mSphereGizmoObj.SetBoxData(radius.RawFloat, center.ToVector3(), mIsFollowTarget);
#endif
        }
        public override void OnRelease()
        {
#if UNITY_EDITOR
            if (mSphereGizmoObj != null && mSphereGizmoObj.gameObject != null)
            {
                GameObject.DestroyImmediate(mSphereGizmoObj.gameObject);
                mSphereGizmoObj = null;
            }
#endif
        }
    }
}
