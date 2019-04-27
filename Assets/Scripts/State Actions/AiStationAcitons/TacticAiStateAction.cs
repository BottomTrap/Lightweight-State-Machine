using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using SA;
using RG;

public class TacticAiStateAction : StateAction
{

    private GameModeManager enemyManager;
    private readonly string actionAiState;
    private readonly string menuState;
    private readonly string tacticState;
    private readonly string transitionState;

    public TacticAiStateAction(GameModeManager enemyManager, string actionAiState, string menuState, string tacticState, string transitionState)
    {
        this.enemyManager = enemyManager;
        this.actionAiState = actionAiState;
        this.menuState = menuState;
        this.tacticState = tacticState;
        this.transitionState = transitionState;
    }

    public override bool Execute()
    {
        Debug.Log("AI CHOICE");
        if (enemyManager.previousState == enemyManager.GetState(transitionState))
        {
            foreach (Transform g in enemyManager.enemyUnitsScript.UnitsList)
            {
                g.GetComponent<AI>().hasPlayed = false;
               // g.GetComponent<AI>().score = 0;
            }
            enemyManager.enemyUnitsScript.commandPoints = enemyManager.enemyUnitsScript.originalCommandPoints;
            Debug.Log(enemyManager.enemyUnitsScript.commandPoints);
        }
        //if (enemyManager.previousState == enemyManager.GetState(actionAiState))
        //{
        //    foreach (Transform g in enemyManager.enemyUnitsScript.UnitsList)
        //    {
        //        g.GetComponent<AI>().score = 0;
        //        Debug.Log("IS THIS WORKING???");
        //    }
        //   
        //}
        if (enemyManager.enemyUnitsScript.commandPoints <= 0 || AllPlayed(enemyManager.enemyUnitsScript.UnitsList))
        {
            enemyManager.SetState(tacticState);
            return true;
        }
        //enemyManager.cameraScript.IsoCameraTransition();




        
        Debug.Log("UNIT NUMBERS " + enemyManager.enemyUnitsScript.UnitsList.Count);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0;
            //Make the menu thingie appear
            return false;
        }

        enemyManager.enemyUnitsScript.GetChildObjectsTransforms();
        enemyManager.enemyUnitsScript.GetPlayersInRange();


        enemyManager.enemyUnitsScript.currentUnit = enemyManager.enemyUnitsScript.ChooseUnitTurn();
        Debug.Log("CHOSEN TRANSFORM SCORE" + " " + enemyManager.enemyUnitsScript.ChooseUnitTurn().name + " " + enemyManager.enemyUnitsScript.ChooseUnitTurn().GetComponent<AI>().score);
        enemyManager.SetState(actionAiState);


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

    bool AllPlayed(List<Transform> unitList)
    {
        foreach (Transform g in unitList)
        {
            if (!g.GetComponent<AI>().hasPlayed)
                return false;
            else continue;
        }
        return true;
    }

}