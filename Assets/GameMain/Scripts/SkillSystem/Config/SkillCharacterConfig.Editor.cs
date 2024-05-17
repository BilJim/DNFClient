using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 技能所属人物配置
/// </summary>
public partial class SkillCharacterConfig
{
#if UNITY_EDITOR

    //临时的 GameObject 主要用于动画播放
    private GameObject mTempCharacter;

    //动画是否处于播放中
    private bool isPlaying;

    //上次运行的时间
    private double lastRunTime;

    //播放中的动画切片
    private Animation animation;

    [ButtonGroup("动画按钮组")]
    [Button("播放", ButtonSizes.Large)]
    [GUIColor(0, 0.8f, 0)]
    public void Play()
    {
        if (skillCharacter == null || skillAnim == null)
            return;
        string characterName = skillCharacter.name;
        //尝试先去 Hierarchy 窗口中寻找该对象
        mTempCharacter = GameObject.Find(characterName);
        if (mTempCharacter == null)
        {
            //没有就创建一个
            mTempCharacter = GameObject.Instantiate(skillCharacter);
            mTempCharacter.name = characterName;
        }

        animation = mTempCharacter.GetComponent<Animation>();
        if (animation.GetClip(skillAnim.name) == null)
            animation.AddClip(skillAnim, skillAnim.name);
        animation.clip = skillAnim;

        animLength = isLoopAnim ? skillAnim.length * loopTimes : skillAnim.length;
        //当前逻辑帧 = 动画时长 / 0.066(每秒15帧)
        logicFrame = (int)(animLength / LogicFrameConfig.LogicFrameInterval);

        //推荐技能时长秒转毫秒
        skillDurationMs = (int)(skillAnim.length * 1000);
        lastRunTime = 0;
        isPlaying = true;

        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.StartPlaySkill();
    }

    [ButtonGroup("动画按钮组")]
    [Button("暂停", ButtonSizes.Large)]
    [GUIColor(1, 1, 0)]
    public void Pause()
    {
        isPlaying = false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.SkillPause();
    }

    [ButtonGroup("动画按钮组")]
    [Button("保存配置", ButtonSizes.Large)]
    [GUIColor(0, 1, 1)]
    public void SaveAssets()
    {
        SkillComplierWindow.GetWindow().SaveSKillData();
    }

    public void OnUpdate(Action progressUpdateCallback = null)
    {
        if (isPlaying)
        {
            if (lastRunTime == 0)
            {
                lastRunTime = EditorApplication.timeSinceStartup;
            }

            //当前运行的时间(时间间隔)
            double curRuntime = EditorApplication.timeSinceStartup - lastRunTime;
            float curAnimNormalizationValue = (float)curRuntime / animLength;
            //计算动画进度
            animProgress = (short)Mathf.Clamp(curAnimNormalizationValue * 100, 0, 100);
            //当前逻辑帧
            logicFrame = (int)(curRuntime / LogicFrameConfig.LogicFrameInterval);
            //采样动画，进行动画播放。理解为编辑器模式下通过 update 更新当前帧下所处的动画帧状态
            animation.clip.SampleAnimation(mTempCharacter, (float)curRuntime);
            if (animProgress == 100)
            {
                //动画播放完成
                PlaySkillEnd();
            }

            progressUpdateCallback?.Invoke();
        }
    }

    public void OnDisable()
    {
        if (mTempCharacter != null)
            GameObject.DestroyImmediate(mTempCharacter);
    }

    public void PlaySkillEnd()
    {
        isPlaying = false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window.PlaySkillEnd();
    }
#endif
}