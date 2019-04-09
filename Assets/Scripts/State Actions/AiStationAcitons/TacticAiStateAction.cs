using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class TacticAiStateAction : StateAction
{

    private EnemyPhaseManager enemyManager;
    private readonly string actionAiState;
    private readonly string menuState;

    public TacticAiStateAction(EnemyPhaseManager enemyManager, string actionAiState, string menuState)
    {
        this.enemyManager = enemyManager;
        this.actionAiState = actionAiState;
        this.menuState = menuState;
    }

    public override bool Execute()
    {
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
