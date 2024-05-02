using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 大厅流程
/// </summary>
public class ProcedureHall : ProcedureBaseScene
{
    private bool m_StartGame = false;
    
    private RoleCreateForm m_HallForm = null;

    /// <summary>
    /// 开始游戏/进入游戏
    /// </summary>
    public void StartGame()
    {
        m_StartGame = true;
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        m_StartGame = false;
        m_formId = GetCurrentSceneData().DefaultFormId;
        //打开默认UI表单
        OpenUIForm(m_formId, this);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        if (m_HallForm != null)
        {
            m_HallForm.Close(true);
            m_HallForm = null;
        }
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (m_StartGame)
        {
            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Battle"));
            procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Common);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }

    private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
        if (ne.UserData != this)
            return;

        m_HallForm = (RoleCreateForm)ne.UIForm.Logic;
    }
}