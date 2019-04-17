using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;


public class EnemyPhaseAction : StateAction
{
    private GameModeManager gameManager;
    
    private readonly string tacticState;
    private readonly string tacticsAiState;


    public EnemyPhaseAction(GameModeManager gameManager, string tacticState, string tacticsAiState)
    {
        this.gameManager = gameManager;
        this.tacticState = tacticState;
        this.tacticsAiState = tacticsAiState;
    }


    public override bool Execute()
    {
        Debug.Log("enemytransition");
       // Debug.Log("HETLI EL SCORE "+gameManager.enemyUnitsScript.UnitsList[0].GetComponent<AI>().score);
         gameManager.SetState(tacticsAiState);
        return true;
    }
}
