using UnityEngine;

public partial class SkillEffectConfig
{
#if UNITY_EDITOR
    
    //Editor模式下克隆的特效对象
    private GameObject mCloneEffect;
    private AnimationAgnet mAnimAgent;
    private ParticlesAgent mParticleAgent;
    //当前逻辑帧
    private int mCurLogicFrame = 0;

    public void GetObjectPath(GameObject obj)
    {
        skillEffectPath= UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillEffectPath:"+ skillEffectPath);
    }

    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        DestroyEffect(); 
        mCurLogicFrame = 0;
    }

    public void SkillPause()
    {
        DestroyEffect();
    }

    /// <summary>
    /// 播放技能结束
    /// </summary>
    public void PlaySkillEnd()
    {
        DestroyEffect();
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        //触发帧创建特效
        if (mCurLogicFrame==triggerFrame)
            CreateEffect();
        //结束帧销毁特效
        else if (mCurLogicFrame == endFrame)
            DestroyEffect();
        mCurLogicFrame++;
    }

    /// <summary>
    /// 创建特效
    /// </summary>
    public void CreateEffect()
    {
        if (skillEffect==null)
            return;
        mCloneEffect= GameObject.Instantiate(skillEffect);
        mCloneEffect.transform.position = SkillComplierWindow.GetCharacterPos();
        //在Editor模式动画文件和粒子特效都不会自动播放，需要我们通过代码进行播放
        mAnimAgent = new AnimationAgnet();
        mAnimAgent.InitPlayAnim(mCloneEffect.transform);
            
        mParticleAgent = new ParticlesAgent();
        mParticleAgent.InitPlayAnim(mCloneEffect.transform);
    }

    /// <summary>
    /// 销毁特效
    /// </summary>
    public void DestroyEffect()
    {
        if (mCloneEffect!=null)
            GameObject.DestroyImmediate(mCloneEffect);
        if (mAnimAgent!=null)
        {
            mAnimAgent.OnDestroy();
            mAnimAgent = null;
        }
        if (mParticleAgent != null)
        {
            mParticleAgent.OnDestroy(); 
            mParticleAgent = null;
        }
    }

#endif
}