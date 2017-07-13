using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : AIState {
	/// <summary>
	/// Take over control of the AI
	/// </summary>
	protected override void TakeControl() { 
		_ship.SetSailState(SailState.None);
	}

	/// <summary>
	/// Called when control of AI is lost
	/// </summary>
	protected override void LoseControl() { }
}
