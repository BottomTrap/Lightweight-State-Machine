using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;

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

    public override bool Execute()
    {
        throw new System.NotImplementedException();
    }
}
