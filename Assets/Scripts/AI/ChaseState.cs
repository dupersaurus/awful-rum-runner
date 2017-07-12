using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : AIState {

	/// <summary>
	/// The ship to chase
	/// </summary>
	[SerializeField]
	private Ship _target;

	/// <summary>
	/// The ship to chase
	/// </summary>
	public Ship target {
		set { _target = value; }
	}
	
	// Update is called once per frame
	void Update () {
		SteerToPoint(_target.position);
	}

	/// <summary>
	/// Take over control of the AI
	/// </summary>
	protected override void TakeControl() {

	}

	/// <summary>
	/// Called when control of AI is lost
	/// </summary>
	protected override void LoseControl() {

	}
}
