using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SailState {
	None,
	Half,
	Full
}

public class SailModel : MonoBehaviour {

	private WindField _windField;

	protected SailState _sailState = SailState.None;

	[SerializeField]
	protected AnimationCurve _pointsOfSail;

	/// <summary>
	/// Force of the wind on the ship is the speed scaled by this
	/// </summary>
	[SerializeField]
	protected float _windMultiplier = 20;

	void Start() {
		_windField = WindField.instance;
	}

	public SailState sailState {
		get { return _sailState; }
		set { _sailState = value; }
	}

	public Vector3 getSailForce() {
		if (_sailState == SailState.None) {
			return Vector3.zero;
		}

		Vector3 windDirection = _windField.GetDirectionAtPosition(transform.position);
		
		// Define 180 degrees as running
		float windAngle = 1 - Quaternion.Angle(transform.rotation, Quaternion.LookRotation(windDirection, Vector3.up)) / 180;
		float windForce = _pointsOfSail.Evaluate(windAngle) * _windField.GetSpeedAtPosition(transform.position) * _windMultiplier;

		if (_sailState == SailState.Half) {
			windForce *= 0.25f;
		}

		return transform.rotation * Vector3.forward * windForce;
	}
}
