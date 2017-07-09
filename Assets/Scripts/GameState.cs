using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	private static GameState _instance;

	private CargoManager _cargoManager;
	private TimeManager _timeManager;
	private CargoHold _cargo;
	private PlayerAssets _assets;

	// Use this for initialization
	void Awake () {
		_instance = this;

		new WindField();

		_cargoManager = new CargoManager();
		_timeManager = new TimeManager();
		_cargo = new CargoHold();
		_assets = new PlayerAssets();
	}
	
	// Update is called once per frame
	void Update () {
		_timeManager.Update(Time.deltaTime);
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
}
