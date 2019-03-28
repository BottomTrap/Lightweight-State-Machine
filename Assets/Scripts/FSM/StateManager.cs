using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public abstract class StateManager : MonoBehaviour
	{
		public State currentState;
        public State previousState;
		protected Dictionary<string, State> allStates = new Dictionary<string, State>();

		public State GetState(string stateId)
		{
			allStates.TryGetValue(stateId, out State retVal);
			return retVal;
		}

		public void SetState(string targetId)
		{
			State targetState = GetState(targetId);
			if (targetState == null)
			{
				Debug.LogError(targetId + " was not found");
			}

            previousState = currentState;
			currentState = targetState;
		}

		private void Start()
		{
			Init();	
		}

		protected abstract void Init();

		protected void FixedTick()
		{
			if (currentState != null)
			{
				currentState.FixedTick();
			}
		}

		protected void Tick()
		{
			if (currentState != null)
			{
				currentState.Tick();
			}
		}

	}
}
