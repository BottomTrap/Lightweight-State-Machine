using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;


public class ActionAiStateAction : StateAction
{
    private readonly EnemyPhaseManager enemyPhaseManager;
    private readonly string tacticAiState;
    private readonly string menuState;

    public ActionAiStateAction (EnemyPhaseManager enemyPhaseManager, string tacticAiState,string menuState)
    {
        this.enemyPhaseManager = enemyPhaseManager;
        this.tacticAiState = tacticAiState;
        this.menuState = menuState;
    }

    public override bool Execute()
    {
        //make sure all the actions are being made 
        //get unto tactics state after the unit finished its actions and some half a second delay
        //IEnumarator? Coroutine? to be determined
        //once it finishes, setState etc 
        return true;

        //the ability to push start during that
        //stop the game and put out a prompt
        //that prompt might be able to put the game unto a whole other scene or restart all of this
        
    }
}
