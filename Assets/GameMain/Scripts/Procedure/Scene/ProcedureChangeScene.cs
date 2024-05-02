using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 场景切换流程
/// 场景和流程一一对应
/// </summary>
public class ProcedureChangeScene : ProcedureBaseScene
{

    private LoadingParams m_loadingParams;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        m_IsChangeSceneComplete = false;

        GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
        GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
        GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

        // 停止所有声音
        GameEntry.Sound.StopAllLoadingSounds();
        GameEntry.Sound.StopAllLoadedSounds();

        // 隐藏所有实体
        GameEntry.Entity.HideAllLoadingEntities();
        GameEntry.Entity.HideAllLoadedEntities();

        // 卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }

        // 还原游戏速度
        GameEntry.Base.ResetNormalGameSpeed();

        m_nextSceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
        IDataTable<DTScene> dtScene = GameEntry.DataTable.GetDataTable<DTScene>();
        DTScene sceneData = dtScene.GetDataRow(m_nextSceneId);
        if (sceneData == null)
        {
            Log.Warning($"场景不存在: {m_nextSceneId}");
            return;
        }
        
        //加载场景
        GameEntry.Scene.LoadScene(sceneData.AssetPath, Constant.AssetPriority.SceneAsset, this);
        m_BackgroundMusicId = sceneData.BackgroundMusicId;
        //弹出场景加载表单
        //打开默认UI表单
        m_loadingParams = LoadingParams.Create(null);
        OpenUIForm(m_loadingFormId, m_loadingParams);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
        GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
        GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

        base.OnLeave(procedureOwner, isShutdown);

        ReferencePool.Release(m_loadingParams);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (!m_IsChangeSceneComplete || !m_loadingParams.LoadingComplete)
            return;
        
        //关闭UI加载表单
        uiComponent.CloseUIForm(loadingFormSerialId);
        //切换场景
        ChangeScene(m_nextSceneId);
    }

    private void OnLoadSceneSuccess(object sender, GameEventArgs e)
    {
        LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
        if (ne.UserData != this)
            return;

        Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

        // if (m_BackgroundMusicId > 0)
        // {
        //     GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
        // }

        m_IsChangeSceneComplete = true;
    }

    private void OnLoadSceneFailure(object sender, GameEventArgs e)
    {
        LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
        if (ne.UserData != this)
            return;

        Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
    }

    private void OnLoadSceneUpdate(object sender, GameEventArgs e)
    {
        LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
        if (ne.UserData != this)
            return;

        Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        m_loadingParams.Progress = ne.Progress;
    }

    private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
    {
        LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
        if (ne.UserData != this)
            return;

        Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName,
            ne.LoadedCount.ToString(), ne.TotalCount.ToString());
    }
}
