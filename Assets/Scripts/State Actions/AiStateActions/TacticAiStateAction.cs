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
		//hides the player CP bar
        states.CP.gameObject.SetActive(false);
		
		//Activates the main menu
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }
        
		
		//Resets the ai hasPlayed bool to flase if the previous state was the transition state
		//also resets the ai command points 
        if (states.previousState == states.GetState(transitionState))
        {
            foreach (Transform g in states.enemyUnitsScript.UnitsList)
            {
                if (g)
                g.GetComponent<AI>().hasPlayed = false;
               
            }
            states.enemyUnitsScript.commandPoints = states.enemyUnitsScript.originalCommandPoints;
           
        }
     



		//Condition of exit for the TacticsAiState
		//Condition is if all Units have played or AI command points reach zero
        if (states.enemyUnitsScript.commandPoints <= 0 || AllPlayed(states.enemyUnitsScript.UnitsList))
        {
            states.CP.gameObject.SetActive(true);
            states.ResetCP();
            states.SetState(tacticState);
            return true;
        }
        




        
        
       
		//Get transform from all child objects from EnemyUnits GameObject (which contains all enemy units)
        states.enemyUnitsScript.GetChildObjectsTransforms();
		
		//Get player units that are in range for all enemy units (in a List)
        states.enemyUnitsScript.GetPlayersInRange();
		
		//Get player units that are in view from all Units
        states.enemyUnitsScript.PlayersInViewTransforms();

		//Choose Unit turn : uses the scoring system to determine which unit to use and what action to do
        states.enemyUnitsScript.currentUnit = states.enemyUnitsScript.ChooseUnitTurn();
        
		//After unit is chosen
        states.SetState(actionAiState);

        
        return true;


        

    }


    //Check if all units have played so we can go back to Player Phase
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