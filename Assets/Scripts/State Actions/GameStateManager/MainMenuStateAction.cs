using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStateAction : StateAction
{
	private GameModeManager statesManager;
	private readonly string tacticState;
	private readonly string cinematicState;

   public MainMenuStateAction(GameModeManager statesManager,string tacticState)
   {
   		this.statesManager = statesManager;
   		this.tacticState= tacticState;
   }
}
