using System;
using GameFramework.Localization;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 启动流程
/// </summary>
public class ProcedureLaunch : ProcedureBase
{

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        // 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言
        InitLanguageSettings();

        // 本地化语言变体配置：根据使用的语言，通知底层加载对应的变体资源
        InitCurrentVariant();

        // 声音配置：根据用户配置数据，设置即将使用的声音选项
        InitSoundSettings();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        // 运行一帧即切换到 Splash 展示流程
        ChangeState<ProcedureSplash>(procedureOwner);
    }

    private void InitLanguageSettings()
    {
        if (GameEntry.Base.EditorResourceMode && GameEntry.Base.EditorLanguage != Language.Unspecified)
            // 编辑器资源模式直接使用 Inspector 上设置的语言
            return;
        Language language = GameEntry.Localization.Language;
        if (GameEntry.Setting.HasSetting(Constant.Setting.Language))
        {
            string languageString = GameEntry.Setting.GetString(Constant.Setting.Language);
            language = (Language)Enum.Parse(typeof(Language), languageString);
        }
        if (language != Language.English && 
            language != Language.ChineseSimplified && language != Language.ChineseTraditional
            && language != Language.Korean)
        {
            // 若是暂不支持的语言，则使用英语
            language = Language.English;

            GameEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
            GameEntry.Setting.Save();
        }

        GameEntry.Localization.Language = language;
        Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
    }

    private void InitCurrentVariant()
    {
        if (GameEntry.Base.EditorResourceMode)
            // 编辑器资源模式不使用 AssetBundle，也就没有变体了
            return;

        string currentVariant = null;
        switch (GameEntry.Localization.Language)
        {
            case Language.English:
                currentVariant = "en-us";
                break;

            case Language.ChineseSimplified:
                currentVariant = "zh-cn";
                break;

            case Language.ChineseTraditional:
                currentVariant = "zh-tw";
                break;

            case Language.Korean:
                currentVariant = "ko-kr";
                break;

            default:
                currentVariant = "zh-cn";
                break;
        }
        //设置资源变体类型
        GameEntry.Resource.SetCurrentVariant(currentVariant);
        Log.Info("Init current variant complete.");
    }

    private void InitSoundSettings()
    {
        //todo
    }
}
