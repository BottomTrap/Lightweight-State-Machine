using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;

public class MenuAction : StateAction
{
   
    private GameModeManager states;
    private readonly string isoState;
    private readonly string perspectiveState;
    private readonly string aimState;
    private readonly string enemyState;
    

    public MenuAction(GameModeManager gameManager, string isoState, string perspectiveState, string aimState,string enemyState)
    {
        this.states = gameManager;
        this.isoState = isoState;
        this.perspectiveState = perspectiveState;
        this.aimState = aimState;
        this.enemyState = enemyState;
        
    }

    public override bool Execute()
    {
        Debug.Log("menu state");
        if(states.previousState == states.GetState("actionState")|| states.previousState == states.GetState("aimState")) { 
            Time.timeScale = 0;
            states.endTurnPrompt.gameObject.SetActive(true);
            if (states.endTurn)
            {
                states.cameraScript.didCameraArrive = false;
                Time.timeScale = 1.0f;
                
                //states.cameraScript.IsoCameraTransition();

                foreach(Transform playerUnits in states.playerUnitsScript.playerUnitsTransformList)
                {
                    playerUnits.GetComponent<PlayerMovement>().distanceTraveled = 0f;
                    
                }

                states.SetState(isoState);
                states.endTurnPrompt.gameObject.SetActive(false);
                return true;

            }
            else
            {
                Time.timeScale = 1.0f;
            }
        
        }

        if (states.previousState == states.GetState("tacticState"))
        {
            Debug.Log("escappe");
            states.endPhasePrompt.gameObject.SetActive(true);
            //Activates the end PHASE Menu prompt, that leads the game into enemy phase
            if (states.endPhase)
            {
                foreach (Transform playerUnits in states.playerUnitsScript.playerUnitsTransformList)
                {
                    playerUnits.GetComponent<PlayerMovement>().distanceTraveled = 0f;
                    states.ResetAP();
                }
                states.commandPoints = states.originalCommandPoints;
                states.endPhasePrompt.gameObject.SetActive(false);
                states.SetState(enemyState);
                return true;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }
        return false;
    }
}
