using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	private static GameState _instance;

	[SerializeField]
	private Ship _playerShip;

	private CargoManager _cargoManager;
	private TimeManager _timeManager;
	private CargoHold _cargo;
	private PlayerAssets _assets;

	private bool _globalPause = false;
	private string _pauseInitiator = null;

	public static bool globalPause {
		get { return _instance._globalPause; }
	}

	// Use this for initialization
	void Awake () {
		_instance = this;

		//new WindField();

		_cargoManager = new CargoManager();
		_timeManager = new TimeManager();
		_cargo = new CargoHold();
		_assets = new PlayerAssets();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_globalPause) {
			_timeManager.Update(Time.deltaTime);
		}
	}

	public static CargoManager cargo {
		get { return _instance._cargoManager; }
	}

	public static CargoHold hold {
		get { return _instance._cargo; }
	}

	public static PlayerAssets assets {
		get { return _instance._assets; }
	}

	public static TimeManager time {
		get { 
			return _instance._timeManager; 
		}
	}

	public static Ship playerShip {
		get { return _instance._playerShip; }
	}

	public static void SetGlobalPause(string id) {
		if (_instance._pauseInitiator != null) {
			return;
		}

		_instance._pauseInitiator = id;
		_instance._globalPause = true;
	}

	public static void ReleaseGlobalPause(string id) {
		if (id == _instance._pauseInitiator) {
			_instance._globalPause = false;
			_instance._pauseInitiator = null;
		}
	} 

	public static void Arrest() {
		_instance.ArrestShip(null);
	}

	private void ArrestShip(Ship ship) {
		_assets.ModifyCash(-_assets.cash);
		_cargo.ClearContents();
	}
}
