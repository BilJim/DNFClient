using UnityEngine;
using UnityEngine.UI;

public class RoleCreateForm : UGuiForm
{
    [SerializeField] private Button close;
    [SerializeField] private Image roleName;
    [SerializeField] private Button enterGame;
    [SerializeField] private InputField playerName;

    private void Awake()
    {
    }


    public void OnStartButtonClick()
    {
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
    }
}