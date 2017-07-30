using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotion : MonoBehaviour {

	[SerializeField]
	private Transform _motionTarget;

	private Ship _ship;

	[SerializeField]
	private float _maxLean = 0;

	private float _currentLean = 0;

	void Awake() {
		_ship = GetComponent<Ship>();
	}

	void LateUpdate () {
		float lean = _currentLean;
		float target = GetDesiredLean();
		float diff = target - lean;
		float leanChange = 10 * Time.deltaTime;

		if (Mathf.Abs(lean) != Mathf.Abs(target)) {
			if (diff >= 0) {
				lean += leanChange;

				if (lean > target) {
					lean = target;
				}
			} else {
				lean -= leanChange;

				if (lean < target) {
					lean = target;
				}
			}
		}

		_currentLean = lean;
		_motionTarget.localRotation = Quaternion.AngleAxis(lean, Vector3.forward);
	}

	float GetDesiredLean() {
		return (_maxLean * Mathf.Clamp01(_ship.velocity.magnitude / 5)) * _ship.rudder;
	}
}
