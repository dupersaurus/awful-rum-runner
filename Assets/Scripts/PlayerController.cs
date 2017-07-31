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

		if (Input.GetButtonDown("Open Map")) {
			UI.UIMain.OpenMap();
			return;
		} 

		// Settlement interaction
		if (Input.GetButtonDown("Open Warehouse")) {
			if (OpenWarehouse()) {
				return;
			}
		} 
		
		if (Input.GetButtonDown("Open Bank")) {
			if (OpenBank()) {
				return;
			}
		}

		if (Input.GetButtonDown("Open Hold")) {
			if (OpenHold()) {
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
		_settlementActionsUI.drawIconsVertical = false;
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

	private bool OpenBank() {
		if (_ship.currentSettlement != null && _ship.currentSettlement.bank != null) {
			_ship.SetSailState(SailState.None);
			UIMain.OpenBank(_ship.currentSettlement.bank);
			return true;
		}

		return false;
	}

	private bool OpenHold() {
		UIMain.OpenCargoHold(GameState.hold);
		return true;
	}

	private void UpdateSettlementIcons() {
		// Warehouse
		var icon = _settlementActionsUI.ToggleIcon("Warehouse Icon", _ship.currentSettlement != null && _ship.currentSettlement.warehouse != null);

		if (icon != null) {
			var button = icon.GetComponent<UnityEngine.UI.Button>();

			if (button == null) {
				button = icon.gameObject.AddComponent<UnityEngine.UI.Button>();
			}

			button.onClick.AddListener(ClickOpenWarehouse);
		}

		// Bank
		icon = _settlementActionsUI.ToggleIcon("Bank Icon", _ship.currentSettlement != null && _ship.currentSettlement.bank != null);

		if (icon != null) {
			var button = icon.GetComponent<UnityEngine.UI.Button>();

			if (button == null) {
				button = icon.gameObject.AddComponent<UnityEngine.UI.Button>();
			}

			button.onClick.AddListener(ClickOpenBank);
		}
	}

	public void ClickOpenWarehouse() {
		OpenWarehouse();
	}

	public void ClickOpenBank() {
		OpenBank();
	}
}
