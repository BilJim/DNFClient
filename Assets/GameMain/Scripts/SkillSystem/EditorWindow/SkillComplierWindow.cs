using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SkillComplierWindow : OdinEditorWindow
{
    [TabGroup("Skill", "角色模型动画数据", SdfIconType.PersonFill, TextColor = "orange")]
    public SkillCharacterConfig character = new();

    [TabGroup("SkillCompiler", "Skill", SdfIconType.Robot, TextColor = "lightmagenta")]
    public SkillConfig skill = new();

    [TabGroup("SkillCompiler", "Damage", SdfIconType.At, TextColor = "lightmagenta")]
    public List<SkillDamageConfig> damageList = new();

    [TabGroup("SkillCompiler", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillEffectConfig> effectList = new();

#if UNITY_EDITOR
    //是否开始播放技能
    private bool isStartPlaySkill = false;

    [MenuItem("Skill/技能编译器")]
    public static SkillComplierWindow ShowWindow()
    {
        return GetWindowWithRect<SkillComplierWindow>(new Rect(0, 0, 1000, 600));
    }

    public static SkillComplierWindow GetWindow()
    {
        return GetWindow<SkillComplierWindow>();
    }
    
    public void SaveSKillData()
    {
        SkillDataConfig.SaveSkillData(character, skill, damageList, effectList);
        Close();
    }
    
    /// <summary>
    /// 加载技能数据
    /// </summary>
    /// <param name="skillData"></param>
    public void LoadSkillData(SkillDataConfig skillData)
    {
        character = skillData.character;
        skill = skillData.skillCfg;
        damageList = skillData.damageCfgList;
        effectList = skillData.effectCfgList;
        // audioList = skillData.audioCfgList;
        // actionList = skillData.actionCfgList;
        // bulletList = skillData.bulletCfgList;
        // buffList = skillData.buffCfgList;
    }

    /// <summary>
    /// 获取Editor模式下角色位置
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCharacterPos()
    {
        if (!HasOpenInstances<SkillComplierWindow>())
            return Vector3.zero;
        SkillComplierWindow window = GetWindow<SkillComplierWindow>();

        if (window.character.skillCharacter != null)
            return window.character.skillCharacter.transform.position;
        return Vector3.zero;
    }

    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        for (var i = 0; i < effectList.Count; i++)
        {
            var item = effectList[i];
            item.StartPlaySkill();
        }

        // foreach (var item in damageList)
        // {
        //     item.PlaySkillStart();
        // }
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
        isStartPlaySkill = true;
    }

    /// <summary>
    /// 技能暂停
    /// </summary>
    public void SkillPause()
    {
        for (var i = 0; i < effectList.Count; i++)
        {
            var item = effectList[i];
            item.PlaySkillEnd();
        }
        // foreach (var item in damageList)
        // {
        //     item.PlaySkillEnd();
        // }
    }

    /// <summary>
    /// 播放技能结束
    /// </summary>
    public void PlaySkillEnd()
    {
        for (var i = 0; i < effectList.Count; i++)
        {
            var item = effectList[i];
            item.PlaySkillEnd();
        }

        // foreach (var item in damageList)
        // {
        //     item.PlaySkillEnd();
        // }
        isStartPlaySkill = false;
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //通过委托帧更新
        EditorApplication.update += OnEditorUpdate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        character.OnDisable();
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        character.OnUpdate(Focus);
        if (isStartPlaySkill)
        {
            OnLogicUpdate();
        }
    }

    //逻辑帧累计运行时间
    private float mAccLogicRuntime;

    //下一个逻辑帧的时间
    private float mNextLogicFrameTime;

    //动画缓动时间 当前帧的增量时间
    private float mDeltaTime;

    //上次更新的时间
    private double mLastUpdateTime;

    /// <summary>
    /// 逻辑Update
    /// </summary>
    private void OnLogicUpdate()
    {
        //模拟帧同步更新 以0.066秒的间隔进行更新
        if (mLastUpdateTime == 0)
            mLastUpdateTime = EditorApplication.timeSinceStartup;
        //计算逻辑帧累计运行时间
        mAccLogicRuntime = (float)(EditorApplication.timeSinceStartup - mLastUpdateTime);
        while (mAccLogicRuntime > mNextLogicFrameTime)
        {
            OnLogicFrameUpdate();
            //下一个逻辑帧的时间
            mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
        }
    }

    private void OnLogicFrameUpdate()
    {
        for (var i = 0; i < effectList.Count; i++)
        {
            var item = effectList[i];
            item.OnLogicFrameUpdate();
        }
        // foreach (var item in damageList)
        // {
        //     item.OnLogicFrameUpdate();
        // }
    }
#endif
}