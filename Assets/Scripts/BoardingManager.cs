using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingManager {
	private static BoardingManager _instance;

	public static BoardingManager instance {
		get {
			if (_instance == null) {
				_instance = new BoardingManager();
			}

			return _instance;
		}
	}

	/// <summary>
	/// The ship initiating the boarding action
	/// </summary>
	private static Ship _actor = null;

	/// <summary>
	/// The ship being boarded
	/// </summary>
	private static Ship _target = null;

	public static bool CanDemandBoarding(Ship actor, Ship target) {
		return _target == null;
	}
	
	/// <summary>
	/// Demand that a ship submit to boarding
	/// </summary>
	/// <param name="actor">The ship doing the boarding</param>
	/// <param name="target">The ship being boarded</param>
	/// <returns>False if a boarding action has already started against the target</returns>
	public static bool DemandBoarding(Ship actor, Ship target) {
		if (_actor != null) {
			return false;
		}

		Debug.LogWarning("Submit to boarding");

		_actor = actor;
		_target = target;

		return true;
	}

	/// <summary>
	/// Begin the boarding action demanded by a ship
	/// </summary>
	/// <param name="actor">The ship that demanded the boarding</param>
	public static void BeginBoardingAction(Ship actor) {
		if (actor != _actor) {
			return;
		}

		Debug.LogWarning("Begin boarding");

		_actor.FullStop();
		_target.FullStop();

		UI.UIMain.OpenBoardingAction();
	}

	/// <summary>
	/// Ends the current boarding action
	/// </summary>
	public static void EndBoardingAction() {

	}

	/// <summary>
	/// Returns the odds for passing the inspection
	/// </summary>
	/// <returns>Number [0,1]</returns>
	public float GetInspectionOdds() {
		if (_target.cargoHold.IsLegal()) {
			return 1;
		}

		int hidingFactor = _target.cargoHold.GetHidingFactor();
		int inspectionSkill = (_actor.crew as HunterCrew).inspectionSkill;

		// even ratio is even odds?
		return (float)hidingFactor / (float)(hidingFactor + inspectionSkill);
	}

	public bool IsLegal() {
		return _target.cargoHold.IsLegal();
	}

	/// <summary>
	/// Resolve the boarding action
	/// </summary>
	public bool ResolveBoarding() {
		float odds = GetInspectionOdds();
		bool pass = Random.value <= odds;

		if (pass) {
			return true;
		}

		

		return false;
	}
}
