using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class PlayerControlsAction : StateAction
{
    private GameModeManager gameStates;
    private string nextState;

    public PlayerControlsAction(GameModeManager states, string nextState)
    {
        this.gameStates = states;
        this.nextState = nextState;
    }


    public override bool Execute()
    {
        throw new System.NotImplementedException();
    }
}
