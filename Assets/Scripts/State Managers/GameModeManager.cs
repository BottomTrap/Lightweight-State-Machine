using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

public class GameModeManager : StateManager
{

    public CameraScript cameraScript;
    public PlayerMovement playerScript;
    public int commandPoints;
    public bool endTurn= false;
    public bool endPhase = false;
    public Transform endTurnPrompt;
    public Transform endPhasePrompt;
    public Transform playerTransform;

    protected override void Init()
    {

        #region Tactics State
        //Tactics State will change all tactics state conditions
        //IN CONDITION: Clicking on the menu prompt to end the turn
        //OUT CONDITION : Clicking on a player to go to action state
        // Clicking on the menu prompt to end the WHOLE PHASE
        State TacticsState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {
                //1// The state Action will activate
                       // PerspectiveCameraChange
                       //CameraFollow
                       //PlayerMovement and general controls
                       new PerspectiveChange(this,"tacticState","actionState","menuState"), 
                  //GO TO ACTION STATE
                //2// The state Action will activate
                    //new MenuAction(this,"tacticState","actionState","aimState"), 
                        //GO TO MENU STATE
                        
            }
            );
        #endregion

        #region Action States

        //Action State will change to action state conditions, like controlling the selected player and attacking once things etc
        //IN CONDITION : Clicking on a player character while CP>0;
        //OUT CONDITION : Clicking on the menu prompt to end the turn (UI) 
        State ActionState = new State(
            new StateAction[]
            {
                new PlayerControlsAction(this,"aimState","tacticState","menuState")
            },
            new StateAction[]
            {
                //1//The action state will activate 
                    //IsoPerspectiveChange
                    //IsoCameraMovement
                //GO TO TACTIC STATE!!
                //new PerspectiveChange(this,"tacticState","actionState","menuState"),
                 
                //2//The stateAction will activate
                    //AimMode Camera Controls
                    // Deactivate player controls
                //new AimAction(), 
                //GO TO AIM STATE

                //3//The stateAction will activate
                    //Deactivate PlayerMovement and controls
                    //Activate Menu Prompt
                    //new MenuAction(this,"tacticState","actionState","aimState"), 
                //GO TO MENU STATE
            }
            );
        #endregion

        #region Aim State
        //Aim State will change the camera into Aim Mode
        //IN CONDITION : being on ActionState and pushing AIM BUTTON
        //OUT CONDITION :// pushing AIM BUTTON
        // Clicking on the menu prompt to end the turn

        State AimState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {
                //1// The stateAction will activate 
                       // Activate player controls
                       //Perspective Camera
                        
                 //ACTIVATE ACTION STATE
                //2// The stateAction will activate
                        //MENU STATE
                        new AimAction(this,"menuState","actionState"), 
            }
            );
        #endregion

        #region Menu State
        //This state is for menu prompts 
        State MenuState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {
                new MenuAction(this,"tacticState","actionState","aimState")
            }
            );

        #endregion


        #region Enemy Phase
        State EnemyPhase = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {

            }
            );
#endregion


        allStates.Add("tacticState",TacticsState);
        allStates.Add("actionState",ActionState);
        allStates.Add("menuState",MenuState);
        allStates.Add("aimState",AimState);
        allStates.Add("EnemyPhase",EnemyPhase);
        
        SetState("tacticState");
    }

    public void EndTurnPrompt()
    {
        endTurn = true;
    }

    public void EndPhasePrompt()
    {
        endPhase = true;
    }

    public void SetTacticsState()
    {
        SetState("tacticState");
    }

    public void SetActionState()
    {
        SetState("actionState");
    }

    public void SetAimState()
    {

    }
    private void FixedUpdate()
    {
        FixedTick();
    }

    private void Update()
    {
        Tick();
        Debug.Log(currentState);
    }
}
