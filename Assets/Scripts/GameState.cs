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

	private Settlement[] _settlements;

	public static Settlement[] settlements {
		get { return _instance._settlements; }
	}

	private Ship[] _ais;

	public static Ship[] ais {
		get { return _instance._ais; }
	}

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

		_settlements = FindObjectsOfType<Settlement>();

		var ais = FindObjectsOfType<AIController>();
		var list = new List<Ship>();

		foreach (var ai in ais) {
			list.Add(ai.GetComponent<Ship>());
		}

		_ais = list.ToArray();
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

	public static float lightLevel {
		get {
			if (_instance != null && _instance._timeManager != null) {
				return _instance._timeManager.lightLevel;
			} else {
				return FindObjectOfType<DayAndNightControl>().lightLevel;
			}
		}
	}

	public static Ship playerShip {
		get { return _instance._playerShip; }
	}

	public static void StartGame() {
		var ais = FindObjectsOfType<AIController>();

		foreach (var ai in ais) {
			ai.Initialize();
		}

		foreach (var settlement in _instance._settlements) {
			settlement.Initialize();
		}

		FindObjectOfType<DayAndNightControl>().day = 1;
		FindObjectOfType<FollowCamera>().enabled = true;
		FindObjectOfType<PlayerController>().Initialize();
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

	public static Ship[] GetShipsInRange(Vector3 pos, float range) {
		var list = new List<Ship>();

		foreach (var ship in _instance._ais) {
			if ((ship.position - pos).magnitude <= range) {
				list.Add(ship);
			}
		}

		return list.ToArray();
	}
}
