using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class PlayerController : MonoBehaviour, IController {

	private bool _initialized = false;

	private Ship _ship = null;

	private UI.WorldSpaceFloater _settlementActionsUI;

	// Use this for initialization
	void Awake () {
		_ship = GetComponent<Ship>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.globalPause || !_initialized) {
			return;
		}

		UpdateSettlementIcons();

		// Settlement interaction
		if (Input.GetButtonDown("Open Warehouse")) {
			if (OpenWarehouse()) {
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

	public void Initialize() {
		_settlementActionsUI = UI.UIMain.CreateEmptyFloater(transform, -0.15f);
		_initialized = true;
	}

	private bool OpenWarehouse() {
		if (_ship.currentSettlement != null && _ship.currentSettlement.warehouse != null) {
			_ship.SetSailState(SailState.None);
			UIMain.OpenWarehouse(_ship.currentSettlement.warehouse);
			return true;
		}

		return false;
	}

	private void UpdateSettlementIcons() {
		var icon = _settlementActionsUI.ToggleIcon("Warehouse Icon", _ship.currentSettlement != null && _ship.currentSettlement.warehouse != null);

		if (icon != null) {
			var button = icon.GetComponent<UnityEngine.UI.Button>();

			if (button == null) {
				button = icon.gameObject.AddComponent<UnityEngine.UI.Button>();
			}

			button.onClick.AddListener(ClickOpenWarehouse);
		}
	}

	public void ClickOpenWarehouse() {
		OpenWarehouse();
	}
}
