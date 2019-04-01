using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class AI_Manager : StateManager
{
    protected override void Init()
    {

        #region AI Tactics State
        State AiTacticsState = new State(
          new  StateAction[]
          {

          },
          new StateAction[]
          {
             
          }
          );
        #endregion

        #region AI Action State
        State AiActionState =new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {

            }
            );
        #endregion

        #region AI Aim State
        State AiAimState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {

            }
            );
        #endregion


        //State MenuState;

        allStates.Add("AiTacticsState", AiTacticsState);
        allStates.Add("AiActionState", AiActionState);
        allStates.Add("AiAimState", AiAimState);


    }

    private void Update()
    {
        Tick();
    }

    private void FixedUpdate()
    {
        FixedTick();
    }
}
