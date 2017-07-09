using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class PlayerController : MonoBehaviour {

	private Ship _ship = null;

	// Use this for initialization
	void Awake () {
		_ship = GetComponent<Ship>();
	}
	
	// Update is called once per frame
	void Update () {
		if (UIMain.hasFocus) {
			return;
		}

		// Settlement interaction
		if (Input.GetButtonDown("Open Warehouse")) {
			if (_ship.currentSettlement.warehouse != null) {
				_ship.SetSailState(SailState.None);
				UIMain.OpenWarehouse(_ship.currentSettlement.warehouse);
				return;
			}
		}

		// Movement
		_ship.SetRudder(Input.GetAxis("Horizontal"));

		if (Input.GetButtonDown("Sail Up")) {
			_ship.SailUp();
		} else if (Input.GetButtonDown("Sail Down")) {
			_ship.SailDown();
		}
	}
}
