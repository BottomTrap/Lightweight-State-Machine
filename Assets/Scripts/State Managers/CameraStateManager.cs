using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using UnityEngine.Experimental.PlayerLoop;
using RG;

public class CameraStateManager : StateManager
{
    public CameraScript cameraScript;
    protected override void Init()
    {
            
        State perspectiveState = new State(
            new StateAction[]
            {

            }, new StateAction[]
            {
                //new PerspectiveChange(this, "isoState"), 
            });
        State isoState = new State(
            new StateAction[]
            {

            }, 
            new StateAction[]
            {
                //new PerspectiveChange(this,"perspectiveState"), 
            });

        allStates.Add("perspectiveState",perspectiveState);
        allStates.Add("isoState", isoState);

        SetState("isoState");

        
    }

    private void FixedUpdate()
    {
        FixedTick();
    }

    private void Update()
    {
        Tick();
    }
    
}
