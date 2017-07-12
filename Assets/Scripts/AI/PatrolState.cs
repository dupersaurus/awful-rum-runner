using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolState : AIState {

	[SerializeField]
	protected List<Vector3> _waypoints;

	private int _currentWaypointIndex = 0;

	void LateUpdate () {
		Vector3 waypoint = _waypoints[_currentWaypointIndex];	
		Vector3 diff = waypoint - _ship.position;

		if (diff.sqrMagnitude <= 1) {
			NextWaypoint();
		} else {
			SteerToWaypoint();
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;

		for (int i = 0; i < _waypoints.Count; i++) {
			Vector3 pos = _waypoints[i];
			pos.y = 1;
			Gizmos.DrawWireSphere(pos, 0.3f);
		}
	}

	/// <summary>
	/// Take over control of the AI
	/// </summary>
	protected override void TakeControl() {
		_ship.SetSailState(SailState.Full);
	}

	/// <summary>
	/// Called when control of AI is lost
	/// </summary>
	protected override void LoseControl() {

	}

	private void NextWaypoint() {
		_currentWaypointIndex++;

		if (_currentWaypointIndex >= _waypoints.Count) {
			_currentWaypointIndex = 0;
		}
	}

	private void SteerToWaypoint() {
		Vector3 waypoint = _waypoints[_currentWaypointIndex];	
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
