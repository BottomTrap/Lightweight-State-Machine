using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class PlayerControlsAction : StateAction
{
    private GameModeManager gameStates;
    private readonly string aimState;
    private readonly string isoState;
    private readonly string menuState;
    private readonly string mainMenuState;

    private PlayerMovement playerMovement;
    private CameraScript cameraScript;
    private PlayerStats playerStats;
    

    public PlayerControlsAction(GameModeManager gameStates, string aimState, string isoState, string menuState, string mainMenuState)
    {
        this.gameStates = gameStates;
        this.aimState = aimState;
        this.isoState = isoState;
        this.menuState = menuState;
        this.mainMenuState = mainMenuState;
    }


    public override bool Execute()
    {

        gameStates.endTurn = false;
       
            playerMovement = gameStates.cameraScript.playerTransform.GetComponent<PlayerMovement>();
            cameraScript = gameStates.cameraScript;
            playerStats = gameStates.cameraScript.playerTransform.GetComponent<PlayerStats>();

        var enemyUnits = gameStates.enemyUnitsScript.UnitsList;
        Debug.Log("this is actionState");
        gameStates.cameraScript.CameraMovement(gameStates.cameraScript.playerTransform);
        //Debug.Log(playerMovement);
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
gameStates.enemyUnitsScript.PlayersInViewTransforms();
        foreach (Transform enemy in enemyUnits)
        {
            
            enemy.GetComponent<AI>().PassiveActions(gameStates.cameraScript.playerTransform);
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
