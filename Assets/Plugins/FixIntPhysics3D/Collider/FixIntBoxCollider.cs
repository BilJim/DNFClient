using FixMath;
using UnityEngine;

namespace FixIntPhysics
{
    public class FixIntBoxCollider : ColliderBehaviour
    {
        private bool mIsFollowTarget;

        //public FixIntVector3 Size;
        /// <summary>
        /// Editor Mode 绘制碰撞体范围工具
        /// </summary>
        public BoxColliderGizmo boxDraw;

        public FixIntBoxCollider(Vector3 size, Vector3 conter)
        {
            this.Size = new FixIntVector3(size);
            this.Conter = new FixIntVector3(conter);
            ColliderType = ColliderType.Box;
        }

        public FixIntBoxCollider(FixIntVector3 size, FixIntVector3 conter)
        {
            this.Size = size;
            this.Conter = conter;
            ColliderType = ColliderType.Box;
        }

        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(FixIntVector3 pos, FixIntVector3 size = default,
            FixInt radius = default)
        {
            base.UpdateColliderInfo(pos, size, radius);
            this.Size = size;
            if (this.boxDraw != null)
            {
                this.boxDraw.transform.position = pos.ToVector3();
            }
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
            this.Size = new FixIntVector3(size);

            if (this.boxDraw != null)
            {
                this.boxDraw.transform.position = pos;
            }
        }

        /// <summary>
        /// 设置碰撞体数据
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="size">宽度</param>
        /// <param name="isFollowTarget">碰撞体绘制是否跟随</param>
        public override void SetBoxData(Vector3 center, Vector3 size, bool isFollowTarget = false)
        {
            SetBoxData(new FixIntVector3(center), new FixIntVector3(size), isFollowTarget);
        }

        public override void SetBoxData(FixIntVector3 center, FixIntVector3 size, bool isFollowTarget = false)
        {
            if (boxDraw == null)
            {
                GameObject obj = new GameObject();
                boxDraw = obj.AddComponent<BoxColliderGizmo>();
                boxDraw.SetBoxData(center.ToVector3(), size.ToVector3(), mIsFollowTarget);
            }

            mIsFollowTarget = isFollowTarget;
            this.Conter = center;
            this.Size = size;

            //boxDraw.transform.localScale = this.Center.x >= 0 ? Vector3.one : new Vector3(-1,1,1);
            boxDraw?.SetBoxData(center.ToVector3(), size.ToVector3(), mIsFollowTarget);
        }

        public override void OnRelease()
        {
            if (boxDraw != null && boxDraw.gameObject != null)
            {
                GameObject.DestroyImmediate(boxDraw.gameObject);
            }

            boxDraw = null;
        }
    }
}