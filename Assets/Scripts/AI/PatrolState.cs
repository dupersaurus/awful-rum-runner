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
		Vector3 lastPos = _waypoints[_waypoints.Count - 1];
		lastPos.y = 1;

		for (int i = 0; i < _waypoints.Count; i++) {
			Vector3 pos = _waypoints[i];
			pos.y = 1;

			Gizmos.DrawWireSphere(pos, 0.3f);
			Gizmos.DrawLine(lastPos, pos);

			lastPos = pos;
		}

		Gizmos.color = Color.magenta;
		Vector3 start = _waypoints[0];
		Gizmos.DrawLine(transform.position + Vector3.up, start + Vector3.up);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Vector3 lastPos = _waypoints[_waypoints.Count - 1];
		lastPos.y = 1;

		for (int i = 0; i < _waypoints.Count; i++) {
			Vector3 pos = _waypoints[i];
			pos.y = 1;

			Gizmos.DrawWireSphere(pos, 0.3f);
			Gizmos.DrawLine(lastPos, pos);

			lastPos = pos;
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
		SteerToPoint(_waypoints[_currentWaypointIndex]);
	}
}
