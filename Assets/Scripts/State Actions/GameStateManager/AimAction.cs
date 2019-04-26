using System.Collections;
using System.Collections.Generic;
using RG;
using UnityEngine;
using SA;

public class AimAction : StateAction
{
    private GameModeManager statesManager;
    private string menuState;
    private string actionState;

    private PlayerMovement playerMovement;

    public AimAction(GameModeManager statesManager, string menuState, string actionState)
    {
        this.statesManager = statesManager;
        this.menuState = menuState;
        this.actionState = actionState;
    }
    public override bool Execute()
    {
        //ACTIVATE CROSSHAIR UI 
        
        playerMovement = statesManager.cameraScript.playerTransform.GetComponent<PlayerMovement>();
        playerMovement.drawcrosshair = true;
        statesManager.cameraScript.AimView();
        playerMovement.transform.GetChild(2).GetComponent<Animator>().SetBool("Aiming", true);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            statesManager.SetState(menuState);
            return true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            playerMovement.drawcrosshair = false;
            playerMovement.transform.GetChild(2).GetComponent<Animator>().SetBool("Aiming", false);
            statesManager.SetState(actionState);
            return true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerMovement.drawcrosshair = false;
            playerMovement.RangedAttack();
        }
        

        return false;
    }
}
