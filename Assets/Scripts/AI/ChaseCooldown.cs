using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCooldown : MonoBehaviour {

	private float _cooldownLeft = 0;
	
	// Update is called once per frame
	void Update () {
		if (!GameState.globalPause) {
			_cooldownLeft -= Time.deltaTime;
		}
	}

	public bool IsActive() {
		return _cooldownLeft > 0;
	}

	public void SetCooldown(float amount) {
		_cooldownLeft = amount;
	}
}
