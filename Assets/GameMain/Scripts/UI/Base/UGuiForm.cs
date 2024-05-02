﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public abstract class UGuiForm : UIFormLogic
{
    public const int DepthFactor = 100;
    private const float FadeTime = 0.3f;

    private static Font s_MainFont = null;
    private Canvas m_CachedCanvas = null;
    private CanvasGroup m_CanvasGroup = null;
    private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();

    public int OriginalDepth { get; private set; }

    public int Depth => m_CachedCanvas.sortingOrder;

    public void Close()
    {
        Close(false);
    }

    public void Close(bool ignoreFade)
    {
        StopAllCoroutines();

        if (ignoreFade)
        {
            GameEntry.UI.CloseUIForm(this);
        }
        else
        {
            StartCoroutine(CloseCo(FadeTime));
        }
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    /// <param name="uiSoundId">背景音id</param>
    public void PlayUISound(int uiSoundId)
    {
        // GameEntry.Sound.PlayUISound(uiSoundId);
    }

    public static void SetMainFont(Font mainFont)
    {
        if (mainFont == null)
        {
            Log.Error("Main font is invalid.");
            return;
        }

        s_MainFont = mainFont;
    }
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
        m_CachedCanvas.overrideSorting = true;
        OriginalDepth = m_CachedCanvas.sortingOrder;

        m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

        RectTransform transform = GetComponent<RectTransform>();
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.anchoredPosition = Vector2.zero;
        transform.sizeDelta = Vector2.zero;
        //相对position初始化
        transform.localPosition = Vector3.zero;

        gameObject.GetOrAddComponent<GraphicRaycaster>();

        Text[] texts = GetComponentsInChildren<Text>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            //避免不设置字体时，文字不显示
            if (s_MainFont != null)
                texts[i].font = s_MainFont;
            if (!string.IsNullOrEmpty(texts[i].text))
                //本地化处理
                texts[i].text = GameEntry.Localization.GetString(texts[i].text);
        }
    }
    
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_CanvasGroup.alpha = 0f;
        StopAllCoroutines();
        StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
    }

    protected override void OnResume()
    {
        base.OnResume();

        m_CanvasGroup.alpha = 0f;
        StopAllCoroutines();
        StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
    }

    protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
    {
        int oldDepth = Depth;
        base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth +
                         OriginalDepth;
        GetComponentsInChildren(true, m_CachedCanvasContainer);
        for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
        {
            m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
        }

        m_CachedCanvasContainer.Clear();
    }

    private IEnumerator CloseCo(float duration)
    {
        yield return m_CanvasGroup.FadeToAlpha(0f, duration);
        GameEntry.UI.CloseUIForm(this);
    }
}