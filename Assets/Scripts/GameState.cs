using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	private CargoHold _cargo;
	private PlayerAssets _assets;

	// Use this for initialization
	void Awake () {
		new WindField();

		_cargo = new CargoHold();
		_assets = new PlayerAssets();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
