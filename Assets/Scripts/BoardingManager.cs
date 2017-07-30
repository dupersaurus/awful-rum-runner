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

	private int _fineBaseMultiplier = 5;

	private int _fineMultiplierMod = 0;

	public int fineModifier {
		get { return _fineBaseMultiplier - _fineMultiplierMod; }
	}

	private bool _hasBoardingPassed = false;

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
		UI.UIMain.CloseScreen();
		_actor = null;
		_target = null;
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
		_hasBoardingPassed = Random.value <= odds;

		_hasBoardingPassed = false;

		return _hasBoardingPassed;
	}

	public bool hasPassedBoarding {
		get { return _hasBoardingPassed; }
	}

	/// <summary>
	/// Apply the penalty for the boarding
	/// </summary>
	public void ApplyBoardingPenalty() {
		int cash = GameState.assets.cash;
		int fine = GetFineCost();

		if (cash >= fine) {
			GameState.assets.ModifyCash(-fine);
			EndBoardingAction();
		} else {
			GameState.Arrest();
		}
	}

	/// <summary>
	/// Cost to bribe
	/// </summary>
	/// <returns></returns>
	public static int GetBribeCost() {
		int baseSkill = (_actor.crew as HunterCrew).bribeSkill;
		float lerp = Mathf.Lerp(0, 10000, (float)baseSkill / 100f);
		return Mathf.RoundToInt(lerp);
	}

	public void BribeCash() {
		int price = GetBribeCost();

		if (price > GameState.assets.cash) {
			return;
		}

		GameState.assets.ModifyCash(-price);
		EndBoardingAction();
	}

	public void BribeReputation() {
		int price = GetBribeCost();

		if (price > GameState.assets.reputation) {
			return;
		}

		GameState.assets.ModifyReputation(-price);
		EndBoardingAction();
	}

	public void EscapeBoarding() {
		if (GameState.assets.reputation < 10000) {
			return;
		}

		GameState.assets.ModifyReputation(-10000);
		EndBoardingAction();
	}

	public void ReduceFine(int amt) {
		int cost = 5000 * amt;

		if (GameState.assets.reputation < cost || amt >= fineModifier) {
			return;
		}

		GameState.assets.ModifyReputation(-cost);
		_fineMultiplierMod += amt;
	}

	public void PayFine() {
		int fine = GetFineCost();

		if (GameState.assets.cash < fine) {
			GameState.Arrest();
		} else {
			GameState.assets.ModifyCash(-fine);
			EndBoardingAction();
		}
	}

	public int GetIllegalCargoSize() {
		var cargos = _target.cargoHold.GetIllegalCargo();
		int total = 0;

		foreach (var item in cargos) {
			total += item.Value;
		}

		return total;
	}

	public int GetIllegalCargoValue() {
		var cargos = _target.cargoHold.GetIllegalCargo();
		int totalFine = 0;

		foreach (var item in cargos) {
			var desc = CargoManager.GetCargo(item.Key);

			totalFine += desc.price * item.Value; 
		}

		return totalFine;
	}

	public int GetFineCost() {
		return GetIllegalCargoValue() * fineModifier;
	}
}
