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

	protected void SteerToPoint(Vector3 waypoint) {
		Vector3 diff = waypoint - _ship.position;

		float angle = Vector3.Angle(_ship.velocity, diff);

		if (angle > 5) {
			
			if (angle > 45) {
				_ship.SetSailState(SailState.Half);
			}

			if (Vector3.Cross(_ship.velocity.normalized, diff).y > 0) {
				_ship.SetRudder(1);
			} else {
				_ship.SetRudder(-1);
			}
		} else {
			_ship.SetSailState(SailState.Full);
			_ship.SetRudder(0);
		}
	}
}
