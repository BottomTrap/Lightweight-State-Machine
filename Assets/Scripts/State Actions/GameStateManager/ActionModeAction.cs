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

        states.endTurn = false;
        
        

        playerMovement = states.cameraScript.playerTransform.GetComponent<PlayerMovement>();
        cameraScript = states.cameraScript;
        playerStats = states.cameraScript.playerTransform.GetComponent<PlayerStats>();


        states.ChangeHp(states.cameraScript.playerTransform);
        states.ResetAP();
        var enemyUnits = states.enemyUnitsScript.UnitsList;
        Debug.Log("this is actionState");
        states.cameraScript.CameraMovement(states.cameraScript.playerTransform);
        
       
        
        playerMovement.Rotate();
        playerMovement.Movement();

        states.UpdateAP(playerMovement.distanceTraveled , playerStats.AP.Value*10);
        

        if (!playerMovement.didHit)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerMovement.Attack();
                playerMovement.didHit = true;
                
            }
        }
       states.enemyUnitsScript.PlayersInViewTransforms();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            states.SetState(menuState);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            states.SetState(aimState);
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
