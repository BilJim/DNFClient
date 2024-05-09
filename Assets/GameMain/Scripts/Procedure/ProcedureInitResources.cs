﻿using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// Package 模式加载资源流程
/// </summary>
public class ProcedureInitResources : ProcedureBase
{
    //判断资源是否初始化完成
    private bool m_InitResourcesComplete = false;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        m_InitResourcesComplete = false;

        // 注意：使用单机模式并初始化资源前，需要先构建 AssetBundle 并复制到 StreamingAssets 中
        GameEntry.Resource.InitResources(OnInitResourcesComplete);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (!m_InitResourcesComplete)
        {
            // 初始化资源未完成则继续等待
            return;
        }

        ChangeState<ProcedurePreload>(procedureOwner);
    }
    
    

    /// <summary>
    /// 单机模式并初始化资源完成时进行回调
    /// </summary>
    private void OnInitResourcesComplete()
    {
        m_InitResourcesComplete = true;
        Log.Info("Init resources complete.");
    }
}