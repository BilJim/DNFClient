using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public static class UIExtension
{
    public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
    {
        float time = 0f;
        float originalAlpha = canvasGroup.alpha;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = alpha;
    }

    public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
    {
        float time = 0f;
        float originalValue = slider.value;
        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(originalValue, value, time / duration);
            yield return new WaitForEndOfFrame();
        }

        slider.value = value;
    }

    public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
    {
        uiComponent.CloseUIForm(uiForm.UIForm);
    }
    
    /// <summary>
    /// 弹出对话框
    /// </summary>
    /// <param name="uiComponent"></param>
    /// <param name="dialogParams"></param>
    public static void OpenDialog(this UIComponent uiComponent, DialogParams dialogParams)
    {
        
    }
}