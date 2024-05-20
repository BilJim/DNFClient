public class EntityStatus
{
    //存活状态
    public SurvivalStatus survivalStatus;
    //行为状态
    public ActionStatus actionStatus;
}

public enum SurvivalStatus
{
    Survival,
    Death
}

public enum ActionStatus
{
    Idle,
    Move,
    //技能释放中
    SkillRelease,
    //浮空
    Float,
    //受击
    Hit,
    //蓄力
    Charging
    
}