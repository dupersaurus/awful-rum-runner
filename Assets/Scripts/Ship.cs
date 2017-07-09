using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	private SailModel _sails;

	[SerializeField]
	private Transform _flag;

	[SerializeField]
	private GameObject _fullSail;

	[SerializeField]
	private GameObject _halfSail;

	[SerializeField]
	private Vector3 _velocity = Vector3.zero;

	[SerializeField]
	private float _drag = 10;

	/// <summary>
	/// The rudder position. Negative is left, positive is right
	/// </summary>
	private float _rudder = 0;

	/// <summary>
	/// Turning rate, deg/sec
	/// </summary>
	[SerializeField]
	private float _turnRate = 30;

	public Vector3 velocity {
		get { return _velocity; }
	}

	// Use this for initialization
	void Awake () {
		_sails = GetComponent<SailModel>();
		SetSailState(SailState.None);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float delta = Time.fixedDeltaTime;

		UpdateFlag();

		// Update ship rotation
		if (_rudder != 0) {
			transform.rotation *= Quaternion.AngleAxis(_turnRate * _rudder * delta, Vector3.up);
		}

		// Update wind and velocity
		Vector3 windForce = _sails.getSailForce();
		float desiredSpeed = windForce.magnitude / _drag;
		float currentSpeed = _velocity.magnitude;

		if (desiredSpeed > currentSpeed) {
			currentSpeed += _drag * delta;

			if (desiredSpeed < currentSpeed) {
				currentSpeed = desiredSpeed;
			}
		} else if (desiredSpeed < currentSpeed) {
			currentSpeed -= _drag * delta;

			if (desiredSpeed > currentSpeed) {
				currentSpeed = desiredSpeed;
			}
		}

		_velocity = transform.rotation * Vector3.forward * currentSpeed;
		
		transform.position += _velocity * delta;
	}

	private void UpdateFlag() {
		Vector3 direction = WindField.instance.GetDirectionAtPosition(transform.position);
		_flag.localRotation = Quaternion.LookRotation(-direction, Vector3.up) * Quaternion.Inverse(transform.rotation);
	}

	public void SailUp() {
		switch (_sails.sailState) {
			case SailState.None:
				SetSailState(SailState.Half);
				break;

			case SailState.Half:
				SetSailState(SailState.Full);
				break;
		}
	}

	public void SailDown() {
		switch (_sails.sailState) {
			case SailState.Full:
				SetSailState(SailState.Half);
				break;

			case SailState.Half:
				SetSailState(SailState.None);
				break;
		}
	}

	public void SetSailState(SailState state) {
		_sails.sailState = state;

		switch (state) {
			case SailState.Full:
				_fullSail.SetActive(true);
				_halfSail.SetActive(true);
				break;

			case SailState.Half:
				_fullSail.SetActive(false);
				_halfSail.SetActive(true);
				break;

			default:
			case SailState.None:
				_fullSail.SetActive(false);
				_halfSail.SetActive(false);
				break;
		}
	}

	public void SetRudder(float rudder) {
		_rudder = rudder;
	}

	/// <summary>
	/// Identify the settlement, if any, the ship is in docking range with
	/// </summary>
	private void IdentifySettlement() {
		GameObject[] settlements = GameObject.FindGameObjectsWithTag("Settlement");
	}
}
