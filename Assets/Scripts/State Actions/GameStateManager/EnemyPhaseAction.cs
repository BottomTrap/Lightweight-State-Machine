using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;
using BehaviorDesigner.Runtime;

public class EnemyPhaseAction : StateAction
{
    private GameModeManager gameManager;
    private string tacticState;

    public EnemyPhaseAction(GameModeManager gameManager, string tacticState)
    {
        this.gameManager = gameManager;
        this.tacticState = tacticState;
    }


    public override bool Execute()
    {
        throw new System.NotImplementedException();
    }
}
