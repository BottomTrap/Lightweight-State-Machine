using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;
using BehaviorDesigner.Runtime;

public class EnemyPhaseAction : StateAction
{
    private GameModeManager gameManager;
    private EnemyPhaseManager enemyAiManager;
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
        if (enemyAiManager.enemyUnitsScript.commandPoints <= 0)
        {
            gameManager.SetState(tacticsAiState);
        }
        else enemyAiManager.SetState(tacticsAiState);



        return true;
    }
}
