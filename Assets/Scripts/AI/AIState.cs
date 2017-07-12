using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour {

	protected AIController _controller;
	protected Ship _ship;

	void Awake() {
		_controller = GetComponent<AIController>();
		_ship = GetComponent<Ship>();

		enabled = false;
	}

	public void Enable() {
		TakeControl();
		enabled = true;
	}

	public void Disable() {
		LoseControl();
		enabled = false;
	}

	/// <summary>
	/// Take over control of the AI
	/// </summary>
	protected abstract void TakeControl();

	/// <summary>
	/// Called when control of AI is lost
	/// </summary>
	protected abstract void LoseControl(); 
}
