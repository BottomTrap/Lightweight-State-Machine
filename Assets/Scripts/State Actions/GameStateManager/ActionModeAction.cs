using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;

public class ActionModeAction : StateAction
{
    private GameModeManager states;
    private readonly string aimState;
    private readonly string isoState;
    private readonly string menuState;
    

    private PlayerMovement playerMovement;
    private CameraScript cameraScript;
    private PlayerStats playerStats;
    

    public ActionModeAction(GameModeManager gameStates, string aimState, string isoState, string menuState)
    {
        this.states = gameStates;
        this.aimState = aimState;
        this.isoState = isoState;
        this.menuState = menuState;
        
    }


    public override bool Execute()
    {
        //Turn is not ended so it's false
        states.endTurn = false;

        //Sets the references for main scripts
        playerMovement = states.cameraScript.playerTransform.GetComponent<PlayerMovement>();
        cameraScript = states.cameraScript;
        playerStats = states.cameraScript.playerTransform.GetComponent<PlayerStats>();

        //Change Hp when it needs to
        states.ChangeHp(states.cameraScript.playerTransform);

        //Resets AP as it's a new turn
        states.ResetAP();


        //Follow the player
        cameraScript.CameraMovement(states.cameraScript.playerTransform);
       
        //Player Controls
        playerMovement.Rotate();
        playerMovement.Movement();

        //Update AP for the current chosen unit
        states.UpdateAP(playerMovement.distanceTraveled , playerStats.AP.Value*10);
        
        //Attack
        if (!playerMovement.didHit)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerMovement.Attack();
                playerMovement.didHit = true;
                
            }
        }

        //enemies update the current units that are in their field of view
       states.enemyUnitsScript.PlayersInViewTransforms();

        //go to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            states.SetState(menuState);
            return true;
        }

        //Go to Aim state
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            states.SetState(aimState);
            return true;
        }
        
        //Go to Main menu screen
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }


        return false;

    }
}
