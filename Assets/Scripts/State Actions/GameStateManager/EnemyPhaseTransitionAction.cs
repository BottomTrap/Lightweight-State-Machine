using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;


public class EnemyPhaseTransitionAction : StateAction
{
    private GameModeManager gameManager;

    private readonly string tacticState;
    private readonly string tacticsAiState;
    


    public EnemyPhaseTransitionAction(GameModeManager gameManager, string tacticState, string tacticsAiState)
    {
        this.gameManager = gameManager;
        this.tacticState = tacticState;
        this.tacticsAiState = tacticsAiState;
        
    }


    public override bool Execute()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            gameManager.mainMenu.gameObject.SetActive(true);
        }
        Debug.Log("enemytransition");
        // Debug.Log("HETLI EL SCORE "+gameManager.enemyUnitsScript.UnitsList[0].GetComponent<AI>().score);
        gameManager.SetState(tacticsAiState);
        return true;

       
    }
}
