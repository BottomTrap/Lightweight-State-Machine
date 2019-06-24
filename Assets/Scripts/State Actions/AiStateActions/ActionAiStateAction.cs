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

        //For MainMenu
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }

        //Checks if there is even a chosen Unit from the TacticsAiState
        if (states.enemyUnitsScript.currentUnit != null)
        {
            
            var AI = states.enemyUnitsScript.currentUnit.GetComponent<AI>();
            //Do the chosen AI action based on the calculated score in the EnemyUnits script
            AI.Action();
           
            
        }
        else states.enemyUnitsScript.commandPoints = 0; //If there is no chosen unit, then there is not usable units so we put command points to zero so we go to playerphase

        
        //If the unit has played, remove one command point and go back to TacticsAiState for the next AI choice
        if (states.enemyUnitsScript.currentUnit.GetComponent<AI>().hasPlayed)
        {
            states.enemyUnitsScript.commandPoints -= 1;
            states.SetState(tacticAiState);
            return true;
        }

        
        return true;

        
        
    }
}
