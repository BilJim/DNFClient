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
        m_ProcedureHall.StartGame();
    }

    public void OnCloseButtonClick()
    {
    }
}