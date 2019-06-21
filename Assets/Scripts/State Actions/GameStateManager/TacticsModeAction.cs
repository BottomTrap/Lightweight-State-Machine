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
        states.cameraScript.IsoCameraTransition();
        Debug.Log("tactics");
        states.enemyUnitsScript.commandPoints = states.enemyUnitsScript.originalCommandPoints;
        
        states.endPhase = false;
        foreach(Transform playerUnits in states.playerUnitsScript.playerUnitsTransformList)
        {
            playerUnits.GetComponent<PlayerMovement>().didHit = false;
        }

        //states.playerTransform = states.cameraScript.PlayerClicked(states.commandPoints);
        
        states.cameraScript.IsoMovement();
       
        if (states.cameraScript.PlayerClicked(states.commandPoints) && states.currentState == states.GetState("tacticState") ) //change key down to player selected or something
        {
            states.cameraScript.khlat = false;
            states.cameraScript.CameraTransition(states.cameraScript.playerTransform);
            states.cameraScript.CameraMovement(states.cameraScript.playerTransform); //I want to update this!!
            states.commandPoints -= 1;
            states.UpdateCP();
            states.SetState(perspectiveState);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) ) //change key down to menu input pressed
        {
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
