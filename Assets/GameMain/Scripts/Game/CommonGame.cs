using UnityGameFramework.Runtime;

public class CommonGame : GameBase
{
    private float m_ElapseSeconds = 0f;

    public override GameMode GameMode => GameMode.Common;

    public override void Initialize()
    {
        base.Initialize();
        
        // GameEntry.Entity.ShowEntity<>(new MyAircraftData(GameEntry.Entity.GenerateSerialId(), 10000)
        // {
        //     Name = "My Aircraft",
        //     Position = Vector3.zero,
        // });
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