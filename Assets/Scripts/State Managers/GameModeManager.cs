using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using RG;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeManager : StateManager
{
    public CameraScript cameraScript;
    public PlayerMovement playerScript;
    public EnemyUnits enemyAiScript;
    public int commandPoints;
    public int originalCommandPoints;
    public bool endTurn = false;
    public bool endPhase = false;
    public bool menu = false;
    public Transform endTurnPrompt;
    public Transform endPhasePrompt;
    public Transform mainMenu;
    public Transform playerTransform;
    public EnemyUnits enemyUnitsScript;
    public PlayerUnits playerUnitsScript;
    public Transform CP;
    public Image APBar;
    public Image HPBar;

    public Image RouninHead;
    public Image KunoichiHead;
    private RectTransform currentPlayerUiTransform;
    private RectTransform nonPlayingUnitUiTransform;
    private Color nonPlayingUnitUiColor;
    private Color currentPlayerUiColor;

    private Transform[] cp;
    public List<Transform> cpList;

    private Vector3 originalApSize;
    private void Awake()
    {
        //this variable to be able to reset command points after each phase
        originalCommandPoints = commandPoints;

        //Stuff to deal with CP UI
        cp = CP.GetComponentsInChildren<Transform>();
        foreach (Transform cpIcon in cp)
        {
            if (cpIcon != CP)
            {
                cpList.Add(cpIcon.transform);
            }
        }

        //Stuff to deal with AP Ui
        originalApSize = APBar.rectTransform.localScale;

        //Stuff to deal with HP Ui
        currentPlayerUiTransform = RouninHead.rectTransform;
        currentPlayerUiColor.a = RouninHead.color.a;
        nonPlayingUnitUiColor.a = KunoichiHead.color.a;
        nonPlayingUnitUiTransform = KunoichiHead.rectTransform;

    }

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
                       new TacticsMode(this,"tacticState","actionState","menuState"),
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
                new ActionModeAction(this,"aimState","tacticState","menuState")
            },
            new StateAction[]
            {
                //1//The action state will activate 
                    //IsoPerspectiveChange
                    //IsoCameraMovement
                //GO TO TACTIC STATE!!
                //new TacticsMode(this,"tacticState","actionState","menuState"),
                 
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

        #region  Menu State
        //This state is for menu prompts 
        State MenuState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {
                new MenuAction(this,"tacticState","actionState","aimState","EnemyPhase")
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
                new EnemyPhaseTransitionAction(this,"tacticState","tacticAiState"),
            }
            );

        State TacticAiState = new State(
            new StateAction[]
            {

            },
            new StateAction[]
            {
                new TacticAiStateAction(this,"actionAiState","menuState","tacticState","EnemyPhase"),
            }
        );
        State ActionAiState = new State(
            new StateAction[]
            {
                new ActionAiStateAction(this,"tacticAiState","menuState"),
            },
            new StateAction[]
            {

            }
        );
        #endregion




        allStates.Add("tacticState", TacticsState);
        allStates.Add("actionState", ActionState);
        allStates.Add("menuState", MenuState);
        allStates.Add("aimState", AimState);
        allStates.Add("EnemyPhase", EnemyPhase);
        allStates.Add("tacticAiState", TacticAiState);
        allStates.Add("actionAiState", ActionAiState);
        

        //SetState("titleScreenState");
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

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        menu = false;
    }

    public void SetTacticsState()
    {
        SetState("tacticState");
    }

    public void SetActionState()
    {
        SetState("actionState");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void UpdateCP()
    {
        cpList[commandPoints].gameObject.SetActive(false);
    } // Updates Command points during player phase

    public void ResetCP()
    {
        foreach (Transform cpIcon in cpList)
        {
            cpIcon.gameObject.SetActive(true);
        }
        Debug.Log(commandPoints);
    }   // Resets Command Points after player phase is over

    public void UpdateAP (float distanceTraveled, float maxAp)
    {
        float currentAp = maxAp - distanceTraveled;
        float ratio = currentAp / maxAp ;
        APBar.rectTransform.localScale = new Vector3(ratio* APBar.rectTransform.localScale.x, APBar.rectTransform.localScale.y, APBar.rectTransform.localScale.z);
    } //Updates AP during player turn (during movement)

    public void ResetAP()
    {
        APBar.rectTransform.localScale= new Vector3(originalApSize.x, originalApSize.y, originalApSize.z);
    } //Resets AP after player TURN is over

    public void UpdateHp(float hpChange , float maxHP) //Updates player HP to show on UI during all turns
    {
        float currentHp = maxHP + hpChange; //NOTE FOR DAMAGE : PARAMETER MUST BE NEGATIVE
        float ratio = currentHp / maxHP;
        HPBar.rectTransform.localScale = new Vector3(ratio * HPBar.rectTransform.localScale.x, HPBar.rectTransform.localScale.y, HPBar.rectTransform.localScale.z);

    }

    public void ChangeHp(Transform currentPlayer) //Changes whose player Unit HP to show during that turn (including the head icon UI) //Change only the head, the bar changes with UpdateHP method
    {
        if (currentPlayer.gameObject.name == "Rounin")
        {
            RouninHead.rectTransform.position = currentPlayerUiTransform.position;
            RouninHead.rectTransform.localScale = currentPlayerUiTransform.localScale;
            RouninHead.color = currentPlayerUiColor;

            KunoichiHead.rectTransform.position = nonPlayingUnitUiTransform.position;
            KunoichiHead.rectTransform.localEulerAngles = nonPlayingUnitUiTransform.localScale;
            KunoichiHead.color = nonPlayingUnitUiColor;
        }
        if (currentPlayer.gameObject.name == "Kunoichi")
        {
            KunoichiHead.rectTransform.position = currentPlayerUiTransform.position;
            KunoichiHead.rectTransform.localEulerAngles = currentPlayerUiTransform.localScale;
            KunoichiHead.color = currentPlayerUiColor;

            RouninHead.rectTransform.position = nonPlayingUnitUiTransform.position;
            RouninHead.rectTransform.localScale = nonPlayingUnitUiTransform.localScale;
            RouninHead.color = nonPlayingUnitUiColor;

        }

    }

    private void FixedUpdate()
    {
        FixedTick();
    }

    private void Update()
    {
        Tick();
        if (!menu)
        {
            Time.timeScale = 1;
        }

        
        
    }
}