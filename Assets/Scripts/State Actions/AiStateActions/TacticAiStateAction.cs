using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using RG;

public class TacticAiStateAction : StateAction
{

    private GameModeManager states;
    private readonly string actionAiState;
    private readonly string menuState;
    private readonly string tacticState;
    private readonly string transitionState;
    

    public TacticAiStateAction(GameModeManager enemyManager, string actionAiState, string menuState, string tacticState, string transitionState)
    {
        this.states = enemyManager;
        this.actionAiState = actionAiState;
        this.menuState = menuState;
        this.tacticState = tacticState;
        this.transitionState = transitionState;
        
    }

    public override bool Execute()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }
        Debug.Log("AI CHOICE");
        if (states.previousState == states.GetState(transitionState))
        {
            foreach (Transform g in states.enemyUnitsScript.UnitsList)
            {
                if (g)
                g.GetComponent<AI>().hasPlayed = false;
               // g.GetComponent<AI>().score = 0;
            }
            states.enemyUnitsScript.commandPoints = states.enemyUnitsScript.originalCommandPoints;
            Debug.Log(states.enemyUnitsScript.commandPoints);
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
        if (states.enemyUnitsScript.commandPoints <= 0 || AllPlayed(states.enemyUnitsScript.UnitsList))
        {
            states.SetState(tacticState);
            return true;
        }
        //enemyManager.cameraScript.IsoCameraTransition();




        
        Debug.Log("UNIT NUMBERS " + states.enemyUnitsScript.UnitsList.Count);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0;
            
            return false;
        }

        states.enemyUnitsScript.GetChildObjectsTransforms();
        states.enemyUnitsScript.GetPlayersInRange();
        states.enemyUnitsScript.PlayersInViewTransforms();


        states.enemyUnitsScript.currentUnit = states.enemyUnitsScript.ChooseUnitTurn();
        Debug.Log("CHOSEN TRANSFORM SCORE" + " " + states.enemyUnitsScript.ChooseUnitTurn().name + " " + states.enemyUnitsScript.ChooseUnitTurn().GetComponent<AI>().score);
        states.SetState(actionAiState);


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
            if (g)
            {
                if (!g.GetComponent<AI>().hasPlayed)
                    return false;
                else continue;
            }
        }
        return true;
    }

}