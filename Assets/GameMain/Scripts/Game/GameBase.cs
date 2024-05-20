using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;

public abstract class GameBase
{
    /// <summary>
    /// 获取游戏模式
    /// </summary>
    public abstract GameMode GameMode { get; }

    /// <summary>
    /// 是否游戏结束
    /// </summary>
    public bool GameOver { get; protected set; }

    //创建UI加载表单时生成的 SerialId
    private int operatingFormSerialId;

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialize()
    {
        GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Shutdown()
    {
        GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
    }

    /// <summary>
    /// 每帧更新
    /// </summary>
    public virtual void Update(float elapseSeconds, float realElapseSeconds)
    {
    }

    protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
    {
        ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
    }

    protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
    {
        ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
        Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
    }


    protected void OpenOperateUI(bool isOpen = true, object userData = null)
    {
        IDataTable<DTUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DTUIForm>();
        DTUIForm formData = dtUIForm.GetDataRow(item => item.AssetName.Equals("Operating"));
        if (isOpen)
        {
            if (!formData.AllowMultiInstance)
            {
                //是否正在加载界面
                if (GameEntry.UI.IsLoadingUIForm(formData.AssetName))
                    return;

                if (GameEntry.UI.HasUIForm(formData.AssetName))
                    return;
            }

            operatingFormSerialId = GameEntry.UI.OpenUIForm(formData.AssetPath, formData.UIGroupName,
                Constant.AssetPriority.UIFormAsset,
                formData.PauseCoveredUIForm, userData);
        }
        else
        {
            GameEntry.UI.CloseUIForm(operatingFormSerialId);
        }
    }
}