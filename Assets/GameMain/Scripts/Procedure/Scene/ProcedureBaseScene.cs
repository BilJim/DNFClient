using System;
using System.Reflection;
using GameFramework.DataTable;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

/// <summary>
/// 场景基础流程类
/// </summary>
public abstract class ProcedureBaseScene : ProcedureBase
{
    /// <summary>
    /// 当前场景 ID
    /// </summary>
    protected int m_sceneId;
    //下一个场景的 sceneId
    protected int m_nextSceneId;
    //场景加载是否完成
    protected bool m_IsChangeSceneComplete = false;
    //场景背景音乐
    protected int m_BackgroundMusicId = 0;
    //场景UI界面ID
    protected int m_formId;

    //UI 组件
    private UIComponent uiComponent;

    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
        uiComponent = GameEntry.UI;
    }

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        //刚进入场景时，NextSceneId 还没改变
        m_sceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
    }

    /// <summary>
    /// 获取当前场景数据
    /// </summary>
    /// <param name="sceneId"></param>
    /// <returns></returns>
    protected DTScene GetCurrentSceneData()
    {
        IDataTable<DTScene> dtScene = GameEntry.DataTable.GetDataTable<DTScene>();
        return dtScene.GetDataRow(m_sceneId);
    }

    /// <summary>
    /// 通过反射切换不同的场景流程
    /// </summary>
    /// <param name="sceneId"></param>
    protected void ChangeScene(int sceneId)
    {
        DTScene sceneData = GameEntry.DataTable.GetDataTable<DTScene>().GetDataRow(sceneId);
        //获取当前正在运行的程序集
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type procedureSceneType = assembly.GetType(sceneData.TypeStr);
        ChangeState(procedureOwner, procedureSceneType);
    }

    protected void OpenUIForm(int formId, object userData = null)
    {
        IDataTable<DTUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DTUIForm>();
        DTUIForm formData = dtUIForm.GetDataRow(formId);
        if (formData == null)
        {
            Log.Warning($"Can not load UI form {formId} from data table");
            return;
        }

        if (!formData.AllowMultiInstance)
        {
            //是否正在加载界面
            if (uiComponent.IsLoadingUIForm(formData.AssetName))
                return;

            if (uiComponent.HasUIForm(formData.AssetName))
                return;
        }

        uiComponent.OpenUIForm(formData.AssetPath, formData.UIGroupName, Constant.AssetPriority.UIFormAsset,
            formData.PauseCoveredUIForm, userData);
    }
}