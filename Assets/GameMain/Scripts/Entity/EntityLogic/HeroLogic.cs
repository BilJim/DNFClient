using UnityGameFramework.Runtime;

public class HeroLogic : TargetableObject
{
    
    public string Name { get; private set; }
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        Name = GameEntry.DataNode.GetData<VarString>("Player.Name");
    }
}