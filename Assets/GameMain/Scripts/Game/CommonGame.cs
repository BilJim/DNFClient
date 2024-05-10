using UnityEngine;
using UnityGameFramework.Runtime;

public class CommonGame : GameBase
{
    private float m_ElapseSeconds = 0f;

    public override GameMode GameMode => GameMode.Common;

    public override void Initialize()
    {
        base.Initialize();

        //typeId 暂时为 1，后期英雄可选择后再设置为动态的
        GameEntry.Entity.ShowHeroEntity(typeof(Hero), "Player", Constant.AssetPriority.HeroAsset,
            new HeroData(GameEntry.Entity.GenerateSerialId(), 1)
            {
                Name = GameEntry.DataNode.GetData<VarString>("Player.Name"),
                Position = Vector3.zero,
            });
        GameOver = false;
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        base.Update(elapseSeconds, realElapseSeconds);

        m_ElapseSeconds += elapseSeconds;
        if (m_ElapseSeconds >= 1f)
        {
        }
    }
}