using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Ship _ship = null;

	// Use this for initialization
	void Awake () {
		_ship = GetComponent<Ship>();
	}
	
	// Update is called once per frame
	void Update () {
		_ship.SetRudder(Input.GetAxis("Horizontal"));

		if (Input.GetButtonDown("Sail Up")) {
			_ship.SailUp();
		} else if (Input.GetButtonDown("Sail Down")) {
			_ship.SailDown();
		}
	}
}
