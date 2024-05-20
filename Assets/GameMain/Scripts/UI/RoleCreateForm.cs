using GameFramework.DataNode;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class RoleCreateForm : UGuiForm
{
    [SerializeField] private Button close;
    [SerializeField] private Image roleName;
    [SerializeField] private Button enterGame;
    [SerializeField] private InputField playerName;

    private ProcedureHall m_ProcedureHall = null;

    private void Start()
    {
        close.onClick.AddListener(OnCloseButtonClick);
        enterGame.onClick.AddListener(OnEnterGameButtonClick);
        playerName.onEndEdit.AddListener(OnNameInputEnd);
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        m_ProcedureHall = (ProcedureHall)userData;
        if (m_ProcedureHall == null)
        {
            Log.Warning("ProcedureHall is invalid when open RoleCreateForm.");
            return;
        }
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        m_ProcedureHall = null;
    }


    public void OnEnterGameButtonClick()
    {
        IDataNode playerNode = GameEntry.DataNode.GetNode("Player.Name");
        if (playerNode == null || string.IsNullOrWhiteSpace(GameEntry.DataNode.GetData<VarString>("Player.Name")))
            return;
        m_ProcedureHall.StartGame();
    }

    public void OnCloseButtonClick()
    {
    }

    public void OnNameInputEnd(string text)
    {
        //通过数据节点设置玩家姓名
        IDataNode dataNode = GameEntry.DataNode.GetOrAddNode("Player");
        dataNode.GetOrAddChild("Name").SetData<VarString>(text);
    }
}