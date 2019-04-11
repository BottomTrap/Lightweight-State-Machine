using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using SA;
using RG;


public class ActionAiStateAction : StateAction
{
    private readonly GameModeManager enemyPhaseManager;
    private readonly string tacticAiState;
    private readonly string menuState;

    public ActionAiStateAction (GameModeManager enemyPhaseManager, string tacticAiState,string menuState)
    {
        this.enemyPhaseManager = enemyPhaseManager;
        this.tacticAiState = tacticAiState;
        this.menuState = menuState;
    }

    public override bool Execute()
    {
        Debug.Log("AI ACTION");
        if (enemyPhaseManager.enemyUnitsScript.currentUnit != null)
        {
            enemyPhaseManager.cameraScript.CameraTransition(enemyPhaseManager.enemyUnitsScript.currentUnit);
            Debug.Log(enemyPhaseManager.enemyUnitsScript.commandPoints);
            var AI = enemyPhaseManager.enemyUnitsScript.currentUnit.GetComponent<AI>();
            //camera follow the action happening
            AI.Action();
            AI.hasPlayed = true;
            enemyPhaseManager.enemyUnitsScript.commandPoints -= 1;
        }
        else enemyPhaseManager.enemyUnitsScript.commandPoints = 0;
        enemyPhaseManager.cameraScript.IsoCameraTransition();
        enemyPhaseManager.cameraScript.IsoMovement();
        Thread.Sleep(1000);
        Debug.Log("wfai wa9t");
        enemyPhaseManager.SetState(tacticAiState);
        //make sure all the actions are being made 
        //get unto tactics state after the unit finished its actions and some half a second delay
        //IEnumarator? Coroutine? to be determined
        //once it finishes, setState etc 
        return true;

        //the ability to push start during that
        //stop the game and put out a prompt
        //that prompt might be able to put the game unto a whole other scene or restart all of this
        
    }
}
