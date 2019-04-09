using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;
public class EnemyPhaseManager : StateManager
{
    public EnemyUnits enemyUnitsScript;
    

    protected override void Init()
    {
        State TacticAiState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {
                
            }
            );
        State ActionAiState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {

            }
            );
        


        allStates.Add("tacticAiState", TacticAiState);
        allStates.Add("actionAiState", ActionAiState);
    }
}
