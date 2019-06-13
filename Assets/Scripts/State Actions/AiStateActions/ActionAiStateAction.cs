using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;


public class ActionAiStateAction : StateAction
{
    private readonly GameModeManager states;
    private readonly string tacticAiState;
    private readonly string menuState;
    



    public ActionAiStateAction (GameModeManager enemyPhaseManager, string tacticAiState,string menuState)
    {
        this.states = enemyPhaseManager;
        this.tacticAiState = tacticAiState;
        this.menuState = menuState;
        
    }

    public override bool Execute()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }
        Debug.Log("AI ACTION");
        if (states.enemyUnitsScript.currentUnit != null)
        {
            //enemyPhaseManager.cameraScript.StartCoroutine(enemyPhaseManager.cameraScript.CameraTransition(enemyPhaseManager.enemyUnitsScript.currentUnit));
            //Debug.Log("command points"+enemyPhaseManager.enemyUnitsScript.commandPoints);
            var AI = states.enemyUnitsScript.currentUnit.GetComponent<AI>();
            //camera follow the action happening
            AI.Action();
            //AI.hasPlayed = true;
            
        }
        else states.enemyUnitsScript.commandPoints = 0;

        //enemyPhaseManager.enemyUnitsScript.currentUnit.GetComponent<AI>().score = 0;

        if (states.enemyUnitsScript.currentUnit.GetComponent<AI>().hasPlayed)
        {
            states.enemyUnitsScript.commandPoints -= 1;
            states.SetState(tacticAiState);
            return true;
        }
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
