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
        Debug.Log("menu state");
        if(gameManager.previousState == gameManager.GetState("actionState")|| gameManager.previousState == gameManager.GetState("aimState")) { 
            Time.timeScale = 0;
            gameManager.endTurnPrompt.gameObject.SetActive(true);
            if (gameManager.endTurn)
            {
            
                Time.timeScale = 1.0f;
                gameManager.cameraScript.IsoCameraTransition();

                gameManager.SetState(isoState);
                gameManager.endTurnPrompt.gameObject.SetActive(false);
                return true;

            }
            else
            {
                Time.timeScale = 1.0f;
            }
        
        }

        //if (gameManager.previousState == gameManager.GetState("actionState"))
        //{
        //    gameManager.cameraScript.IsoCameraTransition();
        //    
        //    gameManager.SetState(isoState);
        //}
        if (gameManager.previousState == gameManager.GetState("tacticState"))
        {
            Debug.Log("escappe");
            gameManager.endPhasePrompt.gameObject.SetActive(true);
            //Activates the end PHASE Menu prompt, that leads the game into enemy phase
            //Leads to "Watch State" Until GAME AI Finishes
            if (gameManager.endPhase)
            {
                gameManager.SetState("EnemyPhase");
                return true;
            }
        }
        return false;
    }
}
