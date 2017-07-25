using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmugglerCrew : Crew {

	private Dictionary<string, UI.WorldSpaceFloater> _spottedSettlements = new Dictionary<string, UI.WorldSpaceFloater>();

	private Dictionary<int, UI.WorldSpaceFloater> _spottedShips = new Dictionary<int, UI.WorldSpaceFloater>();

	// Update is called once per frame
	void Update () {
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

		foreach (var sir in shipsInRange) {
			if (!_spottedShips.ContainsKey(sir.GetInstanceID())) {
				sir.GetComponent<AIController>().ShowFactionFlag();
				_spottedShips.Add(sir.GetInstanceID(), null);
			}
		}
	}
}
