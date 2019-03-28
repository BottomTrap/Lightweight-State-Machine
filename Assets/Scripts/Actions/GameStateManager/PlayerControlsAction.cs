using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class PlayerControlsAction : StateAction
{
    private GameModeManager gameStates;
    private string aimState;
    private string isoState;
    private string menuState;

    private PlayerMovement playerMovement;
    private CameraScript cameraScript;
    private PlayerStats playerStats;
    

    public PlayerControlsAction(GameModeManager gameStates, string aimState, string isoState, string menuState)
    {
        this.gameStates = gameStates;
        this.aimState = aimState;
        this.isoState = isoState;
        this.menuState = menuState;
    }


    public override bool Execute()
    {
        playerMovement = gameStates.cameraScript.playerTransform.GetComponent<PlayerMovement>();
        cameraScript = gameStates.cameraScript;
        playerStats = gameStates.cameraScript.playerTransform.GetComponent<PlayerStats>();
        Debug.Log("this is actionState");
        gameStates.cameraScript.CameraMovement();
        
        //gameStates.cameraScript.playerTransform.GetComponent<PlayerMovement>().Movement();
        playerMovement.Rotate();
        if (playerMovement.distanceTraveled <
            playerStats.AP.Value*10)
        {
            playerMovement.Movement();
        }

        if (!playerMovement.didHit)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerMovement.Attack();
                playerMovement.didHit = true;
                
            }
        }
       // if (gameStates.cameraScript.PlayerClicked(gameStates.commandPoints) != null)
       // {
       //     //gameStates.cameraScript.CameraMovement(gameStates.playerTransform);
       //     //gameStates.cameraScript.CameraTransition(gameStates.playerTransform);
       //     return true;
       // }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameStates.SetState(menuState);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameStates.SetState(aimState);
            return true;
        }
        



        return false;

    }
}
