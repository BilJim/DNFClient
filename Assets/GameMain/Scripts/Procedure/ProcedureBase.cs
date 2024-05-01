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
}