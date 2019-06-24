using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using RG;

public class TacticsMode : StateAction
{
    private GameModeManager states;
    private readonly string isoState;
    private readonly string perspectiveState;
    private readonly string menuState;
    


    public TacticsMode(GameModeManager states, string isoState, string perspectiveState, string menuState)
    {
        this.states = states;
        this.isoState = isoState;
        this.perspectiveState = perspectiveState;
        this.menuState = menuState;
        
    }
    public void Reset(Transform playerUnits)
    {
        playerUnits.GetComponent<PlayerMovement>().didHit = false;
        playerUnits.GetComponent<PlayerMovement>().distanceTraveled = 0f;
    }
    public override bool Execute()
    {
        //Cool Camera Transition
        states.cameraScript.IsoCameraTransition();
        //Reset the enemy's command points 
        states.enemyUnitsScript.commandPoints = states.enemyUnitsScript.originalCommandPoints;
        //endPhase is false which means the user did not chose to end his phase yet
        states.endPhase = false;
        
        //Resets both didHit and distanceTraveled variables , didHit to show that he can still hit and distanceTraveled to recover the AP bar
        states.playerUnitsScript.playerUnitsTransformList.ForEach(Reset);
     

        
        //Camera Control
        states.cameraScript.IsoMovement();
       

        //Sets the ActionState after a player is clicked
        if (states.cameraScript.PlayerClicked(states.commandPoints) && states.currentState == states.GetState("tacticState") ) //change key down to player selected or something
        {
            //Deals with the camera transitionning
            //Removes one CP since the player is playing
            //Updates CP UI
            //Sets the state to action State
            states.cameraScript.didCameraArrive = false;
            states.cameraScript.CameraTransition(states.cameraScript.playerTransform);
            states.commandPoints -= 1;
            states.UpdateCP();
            states.SetState(perspectiveState);
            return true;
        }


        //Resets the distance traveled and didHit Values and resets the AP for the UI (if the previous state was a menu State
        if (states.previousState == states.GetState("menuState"))
        {
            foreach (Transform playerUnits in states.playerUnitsScript.playerUnitsTransformList)
            {

                playerUnits.GetComponent<PlayerMovement>().didHit = false;
                playerUnits.GetComponent<PlayerMovement>().distanceTraveled = 0f;
                states.ResetAP();
            }
        }
       
        //Go to Menu state
        if (Input.GetKeyDown(KeyCode.Escape) ) 
        {
            states.SetState(menuState);
            return true;
        }

        //go automatically to menu state after command points reach zero
        if (states.commandPoints <= 0)
        {
            states.SetState(menuState);
            return true;
        }

        //Go to mainMenu (without UI as it is general)
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }

        //if endPhase (from the UI) then it resets the command Points
       if (states.endPhase)
        {
            states.commandPoints = states.originalCommandPoints;
        }


        return false;
    }
}
