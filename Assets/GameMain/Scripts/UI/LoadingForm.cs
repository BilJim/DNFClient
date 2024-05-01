using UnityEngine;
using UnityEngine.UI;

public class LoadingForm : UGuiForm
{
    //背景图
    [SerializeField] private RawImage bk;

    //进度条滑块
    [SerializeField] private Image slider;

    //提示文本
    [SerializeField] private Text tips;


    public void OnStartButtonClick()
    {
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
    }
}