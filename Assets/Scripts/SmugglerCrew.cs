using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmugglerCrew : Crew {

	private Dictionary<string, UI.WorldSpaceFloater> _spottedSettlements = new Dictionary<string, UI.WorldSpaceFloater>();

	private Dictionary<Ship, UI.WorldSpaceFloater> _spottedShips = new Dictionary<Ship, UI.WorldSpaceFloater>();

	// Update is called once per frame
	void Update () {
		if (GameState.globalPause) {
			return;
		}

		Vector3 pos = GetComponent<Ship>().position;
		float spotSkill = GetSpotDistance();

		// Spot settlements
		Settlement[] settlements = GameState.settlements;
		
		foreach (var settlement in settlements) {
			if (settlement.IsInView(pos, spotSkill)) {
				if (!_spottedSettlements.ContainsKey(settlement.name)) {
					//var icon = UI.UIMain.AddFlagIcon(settlement.transform, settlement.flag);
					settlement.ShowFlag();
					_spottedSettlements.Add(settlement.name, null);
				}
			} else {
				if (_spottedSettlements.ContainsKey(settlement.name)) {
					//Destroy(_spottedSettlements[settlement.name].gameObject);
					settlement.HideFlag();
					_spottedSettlements.Remove(settlement.name);
				}
			}
		}

		// Spot ships
		Ship[] shipsInRange = GameState.GetShipsInRange(pos, spotSkill);
		List<Ship> shipsToRemove = new List<Ship>();

		foreach (var spotted in _spottedShips) {
			bool isSeen = false;

			for (int i = 0; i < shipsInRange.Length; i++) {
				if (shipsInRange[i] == spotted.Key) {
					isSeen = true;
					break;
				}
			}	

			if (!isSeen) {
				spotted.Key.GetComponent<AIController>().HideFactionFlag();
				shipsToRemove.Add(spotted.Key);
			}
		}

		for (int i = 0; i < shipsToRemove.Count; i++) {
			_spottedShips.Remove(shipsToRemove[i]);
		}

		foreach (var sir in shipsInRange) {
			if (!_spottedShips.ContainsKey(sir)) {
				sir.GetComponent<AIController>().ShowFactionFlag();
				_spottedShips.Add(sir, null);
			}
		}
	}
}
