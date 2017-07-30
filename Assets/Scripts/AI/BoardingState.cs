using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingState : ChaseState {

	/// <summary>
	/// Use to calculate whether the target ran
	/// </summary>
	private float _timeOfDemand;

	/// <summary>
	/// Distance from target to begin boarding
	/// </summary>
	[SerializeField]
	private float _boardRadius = 5f;

	public float boardRadius {
		get { return _boardRadius; }
	}

	/// <summary>
	/// Amount of consecutive time spent in boarding radius to start boarding
	/// </summary>
	[SerializeField]
	private float _requiredBoardTime = 6;

	[SerializeField]
	private float _baseBoardTime = 1;

	/// <summary>
	/// Consecutive time spent in board radius
	/// </summary>
	private float _boardTime = 1;

	private UI.WorldSpaceFloater _boardIcon;
	private UI.TimeToBoardFloater _timeToBoardUI;

	protected override void TakeControl() {
		if (BoardingManager.DemandBoarding(_ship, _target)) {
			_timeOfDemand = Time.time;
			_boardTime = _baseBoardTime;
			_controller.AddIcon("Submit Boarding Icon");
		} else {
			_controller.ChangeToState<ChaseState>();
		}
	}

	protected override void LoseControl() {
		if (_timeToBoardUI != null) {
			DestroyImmediate(_timeToBoardUI.gameObject);
			_timeToBoardUI = null;
		}

		_controller.RemoveIcon("Submit Boarding Icon");
	}

	protected override bool CheckForBoarding() {
		Vector3 heading = GetHeadingToTarget();

		if (_boardTime >= 0) {
			if (_timeToBoardUI == null) {
				_timeToBoardUI = UI.UIMain.AddTimeToBoard(_ship.transform);
			}

			_boardTime += GetProgressIncrement() * Time.deltaTime;
			_timeToBoardUI.progress = _boardTime / _requiredBoardTime;

			if (_target.velocity.magnitude <= 0.01f) {
				float distance = GetHeadingToTarget().magnitude;

				if (distance < 3) {
					_ship.SetSailState(SailState.Half);
				} else if (distance < 1.5f) {
					_ship.SetSailState(SailState.None);
				}
			}

			if (_boardTime >= _requiredBoardTime) {
				BoardingManager.BeginBoardingAction(_ship);
				_controller.ChangeToState<WaitState>();
				return true;
			}
		} else {
			GiveUpChase();
		}

		return false;
	}

	protected override Vector3 GetChasePosition() {
		Vector3 pos = _target.position;
		Vector3 targetForward = _target.transform.rotation * Vector3.forward;
		Vector3 diff = _ship.position - pos;
		float dot = Vector3.Dot(diff.normalized, targetForward);

		// In front of target
		if (dot >= 0.7f) {
			pos += targetForward * 1.5f;
		}

		// To the side or behind
		else {
			Vector3 cross = Vector3.Cross(diff.normalized, targetForward);

			// Left
			if (cross.y >= 0) {
				pos += Quaternion.AngleAxis(-90, Vector3.up) * targetForward * 1.5f;
			}

			// Right
			else {
				pos += Quaternion.AngleAxis(90, Vector3.up) * targetForward * 1.5f;
			}
		}

		return pos;
	}

	/// <summary>
	/// Get the amount of increment boarding progress time by. 
	/// Is a function of distance and other stuff.
	/// </summary>
	/// <returns></returns>
	private float GetProgressIncrement() {
		float distance = GetHeadingToTarget().magnitude;

		// If lost sight, decrease by a bunch
		if (distance > GetComponent<Crew>().GetSpotDistance()) {
			return -1f;
		}

		// If out of boarding range, decrease by a bit
		else if (distance > _boardRadius) {
			return -0.25f;
		}

		// Close by and not running away
		else if (_target.velocity.magnitude <= 0.01f && distance < 1) {
			return 10000;
		}

		else {
			if (distance < 1) {
				distance = 1;
			}

			return Mathf.Clamp01((_boardRadius - distance) / _boardRadius + 0.1f);
		}
	}

	private void GiveUpChase() {
		BoardingManager.EndBoardingAction();
		ChaseState chase = _controller.ChangeToState<ChaseState>();
		chase.target = _target;
	}
}
