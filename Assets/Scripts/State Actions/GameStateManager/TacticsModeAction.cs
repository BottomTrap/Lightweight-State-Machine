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

    public override bool Execute()
    {
       
        Debug.Log("tactics");
        states.enemyUnitsScript.commandPoints = states.enemyUnitsScript.originalCommandPoints;
        // §§§§§ MAKE THE NUMBER OF COMMAND POINTS APPEAR ON THE UI
        states.endPhase = false;
        foreach(Transform playerUnits in states.playerUnitsScript.playerUnitsTransformList)
        {
            playerUnits.GetComponent<PlayerMovement>().didHit = false;
        }

        //states.playerTransform = states.cameraScript.PlayerClicked(states.commandPoints);
       states.cameraScript.IsoMovement();
        if (states.cameraScript.PlayerClicked(states.commandPoints) && states.currentState == states.GetState("tacticState") ) //change key down to player selected or something
        {
            states.cameraScript.CameraTransition(states.cameraScript.playerTransform);
            //states.cameraScript.CameraMovement(states.cameraScript.PlayerClicked(states.commandPoints)); //I want to update this!!
            //Debug.Log(states.currentState);
            states.commandPoints -= 1;
            // §§§§ DO THE COMMAND POINTS REMOVE FROM THE UI
            states.SetState(perspectiveState);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) ) //change key down to menu input pressed
        {
            //Debug.Log("ortho state");
            //states.cameraScript.orthoOn = true;
            //states.cameraScript.IsoCameraTransition();
            states.SetState(menuState);
            return true;
        }
        if (states.commandPoints <= 0)
        {
            states.SetState(menuState);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }
       


        return false;
    }
}
