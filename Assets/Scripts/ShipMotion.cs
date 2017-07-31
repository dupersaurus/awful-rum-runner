using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Range {
	public float min = 0;
	public float max = 1;

	public float Lerp(float range) {
		return min + (max - min) * range;
	} 

	public float GetPct(float value) {
		return (Mathf.Clamp(value, min, max) - min) / (max - min);
	}
}

public class ShipMotion : MonoBehaviour {

	[SerializeField]
	private Transform _motionTarget;

	private Ship _ship;

	[SerializeField]
	private float _maxLean = 0;

	private float _currentLean = 0;

	[SerializeField]
	private AudioSource _windSound;

	[SerializeField]
	private Range _windVolumeRange;

	[SerializeField]
	private Range _windSpeedRange;

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
		_motionTarget.localRotation = Quaternion.Euler(Mathf.Sin(Time.time + 1) * 2, 0, lean); //Quaternion.AngleAxis(lean, Vector3.forward);

		// Speed sound
		if (_windSound != null) {
			float speed = _windSpeedRange.GetPct(_ship.velocity.magnitude);
			float volume = _windVolumeRange.Lerp(speed);
			_windSound.volume = volume;
		}
	}

	float GetDesiredLean() {
		float turnLean = (_maxLean * Mathf.Clamp01(_ship.velocity.magnitude / 5)) * _ship.rudder;
		float bobLean = Mathf.Sin(Time.time) * 3;

		return turnLean + bobLean;
	}
}
