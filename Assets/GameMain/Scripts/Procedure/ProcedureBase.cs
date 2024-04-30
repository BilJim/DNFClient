using System;
using System.Reflection;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;

public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase
{

    protected IFsm<IProcedureManager> procedureOwner;
    
    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
        this.procedureOwner = procedureOwner;
    }

    /// <summary>
    /// 通过反射切换不同的场景流程
    /// </summary>
    /// <param name="sceneId"></param>
    protected void ChangeScene(int sceneId)
    {
        DTScene sceneData = GameEntry.DataTable.GetDataTable<DTScene>().GetDataRow(sceneId);
        //获取当前正在运行的程序集
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type procedureSceneType = assembly.GetType(sceneData.TypeStr);
        ChangeState(procedureOwner, procedureSceneType);
    }
}