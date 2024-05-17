using UnityEditor;
using UnityEngine;

/// <summary>
/// 技能配置
/// </summary>
public partial class SkillConfig
{
#if UNITY_EDITOR
    public void GetObjectPath(GameObject obj)
    {
        skillHitEffectPath = AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillHitEffectPath:" + skillHitEffectPath);
    }
#endif
}