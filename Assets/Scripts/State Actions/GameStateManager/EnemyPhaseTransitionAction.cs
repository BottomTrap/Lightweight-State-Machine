using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;


public class EnemyPhaseTransitionAction : StateAction
{
    private GameModeManager states;

    private readonly string tacticState;
    private readonly string tacticsAiState;
    


    public EnemyPhaseTransitionAction(GameModeManager gameManager, string tacticState, string tacticsAiState)
    {
        this.states = gameManager;
        this.tacticState = tacticState;
        this.tacticsAiState = tacticsAiState;
        
    }


    public override bool Execute()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }
        Debug.Log("enemytransition");
        // Debug.Log("HETLI EL SCORE "+gameManager.enemyUnitsScript.UnitsList[0].GetComponent<AI>().score);
        states.SetState(tacticsAiState);
        return true;

       
    }
}
