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
	private float _boardRadius = 2.5f;

	public float boardRadius {
		get { return _boardRadius; }
	}

	/// <summary>
	/// Amount of consecutive time spent in boarding radius to start boarding
	/// </summary>
	[SerializeField]
	private float _requiredBoardTime = 3;

	/// <summary>
	/// Consecutive time spent in board radius
	/// </summary>
	private float _boardTime = 0;

	private UI.WorldSpaceFloater _boardIcon;
	private UI.TimeToBoardFloater _timeToBoardUI;

	protected override void TakeControl() {
		if (BoardingManager.DemandBoarding(_ship, _target)) {
			_timeOfDemand = Time.time;
			_boardIcon = UI.UIMain.DemandBoarding(_ship.transform);
		} else {
			_controller.ChangeToState<ChaseState>();
		}
	}

	protected override void LoseControl() {
		DestroyImmediate(_timeToBoardUI);
		DestroyImmediate(_boardIcon);
	}

	protected override bool CheckForBoarding() {
		Vector3 heading = GetHeadingToTarget();

		if (heading.magnitude <= _boardRadius) {
			if (_timeToBoardUI == null) {
				_timeToBoardUI = UI.UIMain.AddTimeToBoard(_ship.transform);
			}

			_boardTime += Time.deltaTime;
			_timeToBoardUI.progress = _boardTime / _requiredBoardTime;

			if (_boardTime >= _requiredBoardTime) {
				BoardingManager.BeginBoardingAction(_ship);
				_controller.ChangeToState<WaitState>();
				return true;
			}
		} else {
			_boardTime = 0;
		}

		return false;
	}
}
