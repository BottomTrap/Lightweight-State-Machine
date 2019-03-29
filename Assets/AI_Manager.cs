using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class AI_Manager : StateManager
{
    protected override void Init()
    {
        State AiTacticsState = new State(
          new  StateAction[]{

        },
          new StateAction[]
          {

          }
          );

        State AiActionState =new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {

            }
            );

        State AiAimState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {

            }
            );

        //State MenuState;
    }
}
