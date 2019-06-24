using System.Collections;
using System.Collections.Generic;
using RG;
using UnityEngine;

public class AimAction : StateAction
{
    private GameModeManager states;
    private string menuState;
    private string actionState;
    

    private PlayerMovement playerMovement;

    public AimAction(GameModeManager statesManager, string menuState, string actionState)
    {
        this.states = statesManager;
        this.menuState = menuState;
        this.actionState = actionState;
        
    }
    public override bool Execute()
    {
        //ACTIVATE CROSSHAIR UI 
        
        playerMovement = states.cameraScript.playerTransform.GetComponent<PlayerMovement>();
        playerMovement.drawcrosshair = true;
        //Setting Aim Camera
        states.cameraScript.AimView();
        
        //To go to menu state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            states.SetState(menuState);
            return true;
        }


        //To deactive crosshair
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            playerMovement.drawcrosshair = false;
            //playerMovement.transform.GetChild(2).GetComponent<Animator>().SetBool("Aiming", false);
            states.SetState(actionState);
            return true;
        }


        //To Attack
        if (!playerMovement.didHit)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerMovement.drawcrosshair = false;
                playerMovement.RangedAttack();
            }
        }

        //For Main Menu
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0;
            states.mainMenu.gameObject.SetActive(true);
        }

        return false;
    }
}
