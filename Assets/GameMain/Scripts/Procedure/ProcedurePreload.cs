using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class ProcedurePreload : ProcedureBase
{

    private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
        GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
        GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
        GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

        m_LoadedFlag.Clear();

        PreloadResources();
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
        GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
        GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
        GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        //所有数据项都加载完成才能切流程
        foreach (KeyValuePair<string, bool> loadedFlag in m_LoadedFlag)
        {
            if (!loadedFlag.Value)
                return;
        }
        //首先加载大厅场景
        procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Hall"));
        ChangeState<ProcedureChangeScene>(procedureOwner);
    }

    private void PreloadResources()
    {
        // 加载全局配置
        LoadConfig("DefaultConfig");

        //首先先加载基础表，基础表加载成功后继续加载需要加载的记录的其他表
        LoadDataTable("BaseTable");

        //加载本地化语言
        LoadLocalization("Default");

        //统一设置字体
        LoadFont("MainFont");
    }

    private void LoadConfig(string configName)
    {
        string configAssetName = AssetUtility.GetConfigAsset(configName, false);
        m_LoadedFlag.Add(configAssetName, false);
        GameEntry.Config.ReadData(configAssetName, this);
    }

    private void LoadDataTable(string dataTableName)
    {
        //从二进制文件中加载数据
        string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, true);
        m_LoadedFlag.Add(dataTableAssetName, false);
        GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
    }

    private void LoadLocalization(string dictionaryName)
    {
        string dictionaryAssetName = AssetUtility.GetDictionaryAsset(dictionaryName, false);
        m_LoadedFlag.Add(dictionaryAssetName, false);
        GameEntry.Localization.ReadData(dictionaryAssetName, this);
    }

    private void LoadFont(string fontName)
    {
        m_LoadedFlag.Add($"Font.{fontName}", false);
        GameEntry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
            {
                m_LoadedFlag[$"Font.{fontName}"] = true;
                UGuiForm.SetMainFont((Font)asset);
                Log.Info("Load font '{0}' OK.", fontName);
            },

            (assetName, status, errorMessage, userData) =>
            {
                Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage);
            }));
    }

    private void OnLoadConfigSuccess(object sender, GameEventArgs e)
    {
        LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        m_LoadedFlag[ne.ConfigAssetName] = true;
        Log.Info("Load config '{0}' OK.", ne.ConfigAssetName);
    }

    private void OnLoadConfigFailure(object sender, GameEventArgs e)
    {
        LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigAssetName, ne.ConfigAssetName, ne.ErrorMessage);
    }

    private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
    {
        LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        m_LoadedFlag[ne.DataTableAssetName] = true;
        Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        //继续加载其余的表
        if (ne.DataTableAssetName.Contains("BaseTable"))
        {
            List<string> needLoadTabs = GameEntry.DataTable.GetDataTable<DTBaseTable>().GetDataRow(0).TableNames;
            foreach (string dataTableName in needLoadTabs)
            {
                LoadDataTable(dataTableName);
            }
        }
    }

    private void OnLoadDataTableFailure(object sender, GameEventArgs e)
    {
        LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
    }

    private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
    {
        LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        m_LoadedFlag[ne.DictionaryAssetName] = true;
        Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
    }

    private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
    {
        LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName, ne.DictionaryAssetName, ne.ErrorMessage);
    }
}