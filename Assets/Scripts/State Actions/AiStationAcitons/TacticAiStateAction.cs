using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using SA;
using RG;

public class TacticAiStateAction : StateAction
{

    private GameModeManager enemyManager;
    private readonly string actionAiState;
    private readonly string menuState;
    private readonly string tacticState;

    public TacticAiStateAction(GameModeManager enemyManager, string actionAiState, string menuState,string tacticState)
    {
        this.enemyManager = enemyManager;
        this.actionAiState = actionAiState;
        this.menuState = menuState;
        this.tacticState = tacticState;
    }

    public override bool Execute()
    {
        
        Delay delay = new Delay(5.0f);
        
        if (enemyManager.previousState == enemyManager.GetState("tacticState"))
        {
            foreach (Transform g in enemyManager.enemyUnitsScript.UnitsList)
            {
                g.GetComponent<AI>().hasPlayed = false;
               
            }
        }
        Debug.Log("AI CHOICE");
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0;
            //Make the menu thingie appear
            return false;
        }

        enemyManager.enemyUnitsScript.GetChildObjectsTransforms();
        enemyManager.enemyUnitsScript.GetPlayersInRange();
        Thread.Sleep(1000);
        if (enemyManager.enemyUnitsScript.ChooseUnitTurn() != null)
        {
            enemyManager.enemyUnitsScript.currentUnit = enemyManager.enemyUnitsScript.ChooseUnitTurn();
        }
            enemyManager.SetState(actionAiState);
        
        if (enemyManager.enemyUnitsScript.commandPoints <= 0)
        {
            enemyManager.SetState(tacticState);
        }
        //choose the enemy unit
        //do camera thing to it
        //go to ActionAiState
        return true;

        
        //if esc or smth is pressed :
        //pause game (time.timescale = 0)
        //go to menuState (zeyda , so nooooo)
        //show menu 
        //return true

        //if s or smth is being pressed
        //do the speed up thing (time.timescale =1.1 or smth)

    }

    

}
