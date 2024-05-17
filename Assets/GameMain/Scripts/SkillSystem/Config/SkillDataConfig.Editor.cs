using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
# if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SkillConfig", menuName = "SkillConfig", order = 0)]
public partial class SkillDataConfig : ScriptableObject
{
#if UNITY_EDITOR

    private static string AssetPath = "Assets/GameMain/ScriptableData/SkillSystem";

    [Button("配置技能", ButtonSizes.Large), GUIColor("green")]
    public void ShowSkillWindowButtonClick()
    {
        SkillComplierWindow window = SkillComplierWindow.ShowWindow();
        window.LoadSkillData(this);
    }

    public static void SaveSkillData(SkillCharacterConfig characterCfg, SkillConfig skillCfg,
        List<SkillDamageConfig> damageCfgList, List<SkillEffectConfig> effectCfgList)
    {
        //通过代码创建SkillDataConfig的实例，并对字段进行赋值储存
        SkillDataConfig skillDataCfg = CreateInstance<SkillDataConfig>();
        skillDataCfg.character = characterCfg;
        skillDataCfg.skillCfg = skillCfg;
        skillDataCfg.damageCfgList = damageCfgList;
        skillDataCfg.effectCfgList = effectCfgList;
        // skillDataCfg.audioCfgList = audioCfgList;
        // skillDataCfg.actionCfgList = actionCfgList;
        // skillDataCfg.bulletCfgList = bulletCfgList;
        // skillDataCfg.buffCfgList = buffCfgList;
        //把当前实例储存为.asset资源文件，当作技能配置
        string assetPath = $"{AssetPath}/{skillCfg.skillId}.asset";
        //如果资源对象已存在，先进行删除，在进行创建
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.CreateAsset(skillDataCfg, assetPath);
    }

    public void SaveAsset()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}