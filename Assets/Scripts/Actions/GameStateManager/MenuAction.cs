using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class MenuAction : StateAction
{
   
    private GameModeManager gameManager;
    private readonly string isoState;
    private readonly string perspectiveState;
    private readonly string aimState;

    public MenuAction(GameModeManager gameManager, string isoState, string perspectiveState, string aimState)
    {
        this.gameManager = gameManager;
        this.isoState = isoState;
        this.perspectiveState = perspectiveState;
        this.aimState = aimState;
    }

    public override bool Execute()
    {
        if (gameManager.endTurn = false)
        {
            return false;
        }
        else
        {
           gameManager.SetState(isoState);
           return true;
        }
    }
}
