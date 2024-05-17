
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class SkillDataConfig : ScriptableObject
{
    //角色数据配置
    public SkillCharacterConfig character;
    //技能基础数据配置
    public SkillConfig skillCfg;
    //技能伤害配置列表
    public List<SkillDamageConfig> damageCfgList;
    //技能特效配置列表
    public List<SkillEffectConfig> effectCfgList;
    //技能音效配置列表
    // public List<SkillAudioConfig> audioCfgList;
    // //技能音效配置列表
    // public List<SkillBulletConfig> bulletCfgList;
    //行动配置列表
    public List<SkillActionConfig> actionCfgList;
    //buff配置列表
    // public List<SkillBuffConfig> buffCfgList;
}