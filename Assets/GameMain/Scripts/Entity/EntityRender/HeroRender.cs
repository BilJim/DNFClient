using FixMath;
using UnityEngine;

public class HeroRender : EntityRender
{
    private HeroLogic m_heroLogic;
    
    public override void OnCreate()
    {
        m_heroLogic = LogicObj as HeroLogic;
        JoystickUGUI.OnMoveCallBack += OnJoyStickMove;
    }

    public override void OnRelease()
    {
        JoystickUGUI.OnMoveCallBack -= OnJoyStickMove;
    }

    private void Update()
    {
        UpdatePosition();
        UpdateDir();
    }

    private void UpdatePosition()
    {
    }

    private void UpdateDir()
    {
    }

    private void OnJoyStickMove(Vector3 inputDir)
    {
        //逻辑方向
        FixIntVector3 logicDir;

        if (inputDir != Vector3.zero)
            logicDir = new FixIntVector3(inputDir);
        else
            logicDir = FixIntVector3.zero;
        
        m_heroLogic.InputLogicFrameEvent(logicDir);
        
    }
    
    
}