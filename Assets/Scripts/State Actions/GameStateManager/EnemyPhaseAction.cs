using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;
using BehaviorDesigner.Runtime;

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
        if (gameManager.enemyUnitsScript.commandPoints <= 0)
        {
            gameManager.SetState(tacticsAiState);
            return true;
        }
         gameManager.SetState(tacticsAiState);
        return true;
    }
}
