using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : AIState {

	/// <summary>
	/// The ship to chase
	/// </summary>
	[SerializeField]
	protected Ship _target;

	/// <summary>
	/// The ship to chase
	/// </summary>
	public Ship target {
		set { _target = value; }
	}

	[SerializeField]
	private float _boardingDistance = 3;

	public float boardingDistance {
		get { return _boardingDistance; }
	}
	
	// Update is called once per frame
	protected void Update () {
		if (!CheckForBoarding()) {
			SteerToPoint(_target.position);
		}
	}

	/// <summary>
	/// Check for being close enough to demand boarding
	/// </summary>
	/// <returns>True if close enough</returns>
	protected virtual bool CheckForBoarding() {
		Vector3 heading = GetHeadingToTarget();

		if (heading.sqrMagnitude <= _boardingDistance * _boardingDistance) {
			if (BoardingManager.CanDemandBoarding(_ship, _target)) {
				BoardingState boarding = _controller.ChangeToState<BoardingState>();
				boarding.target = _target;
			}
		}

		return false;
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

	protected Vector3 GetHeadingToTarget() {
		return _target.position - _ship.position;
	}
}
