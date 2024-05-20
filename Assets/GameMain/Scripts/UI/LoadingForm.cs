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

    //加载表单内容项
    private LoadingParams loadingParams;
    //最大进度
    private float maxProgress = 1;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        loadingParams = (LoadingParams)userData;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        slider.fillAmount = loadingParams.Progress;
        if (loadingParams.Progress <= 0.89)
            return;
        if (loadingParams.Progress < maxProgress)
        {
            loadingParams.Progress++;
            return;
        }
        loadingParams.LoadingComplete = true;
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        loadingParams = null;
    }
}