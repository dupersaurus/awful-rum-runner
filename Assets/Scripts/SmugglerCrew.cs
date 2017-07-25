using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmugglerCrew : Crew {

	private Dictionary<string, UI.WorldSpaceFloater> _spottedSettlements = new Dictionary<string, UI.WorldSpaceFloater>();

	// Update is called once per frame
	void Update () {
		Settlement[] settlements = GameState.settlements;
		Vector3 pos = GetComponent<Ship>().position;

		foreach (var settlement in settlements) {
			if (settlement.IsInView(pos, baseSpottingSkill)) {
				if (!_spottedSettlements.ContainsKey(settlement.name)) {
					var icon = UI.UIMain.AddFlagIcon(settlement.transform, settlement.flag);
					_spottedSettlements.Add(settlement.name, icon);
					Debug.Log("Spotted settlement " + settlement.name);
				}
			} else {
				if (_spottedSettlements.ContainsKey(settlement.name)) {
					Destroy(_spottedSettlements[settlement.name].gameObject);
					_spottedSettlements.Remove(settlement.name);
					Debug.Log("Lost settlement " + settlement.name);
				}
			}
		}
	}
}
