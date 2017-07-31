using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	private IController _controller;

	private SailModel _sails;

	[SerializeField]
	private Faction _faction;

	public Faction faction {
		get { return _faction; }
	}

	[SerializeField]
	private Transform _flag;

	[SerializeField]
	private GameObject _fullSail;

	[SerializeField]
	private GameObject _halfSail;

	[SerializeField]
	private Vector3 _velocity = Vector3.zero;

	[SerializeField]
	private float _mass = 10;

	[SerializeField]
	private float _drag = 2;

	/// <summary>
	/// The rudder position. Negative is left, positive is right
	/// </summary>
	private float _rudder = 0;

	[SerializeField]
	private ParticleSystem _wake;

	public float rudder {
		get { return _rudder; }
	}

	/// <summary>
	/// Turning rate, deg/sec
	/// </summary>
	[SerializeField]
	private float _turnRate = 30;

	private Settlement _currentSettlement = null;

	public Settlement currentSettlement {
		get { return _currentSettlement; }
	}

	public Vector3 velocity {
		get { return _velocity; }
	}

	public Vector3 position {
		get { return transform.position; }
	}

	public float mass {
		get { 
			if (_controller is PlayerController) {
				return _mass + GameState.hold.currentCargo / 100;
			} else {
				return _mass;
			}
		}
	}

	public CargoHold cargoHold {
		get { return GameState.hold; }
	}

	public Crew crew {
		get { return GetComponent<Crew>(); }
	}

	void OnDrawGizmos() {
		ChaseState chase = GetComponent<ChaseState>();

		if (chase != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(position, chase.boardingDistance);
		}

		BoardingState boarding = GetComponent<BoardingState>();

		if (boarding != null) {
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(position, boarding.boardRadius);
		}
	}

	// Use this for initialization
	void Awake () {
		_controller = GetComponent<IController>();
		_sails = GetComponent<SailModel>();
		SetSailState(SailState.None);

		if (_wake != null) {
			_wake.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameState.globalPause) {
			return;
		}

		float delta = Time.fixedDeltaTime;

		UpdateFlag();

		// Update ship rotation
		if (_rudder != 0) {
			transform.localRotation *= Quaternion.AngleAxis(_turnRate * _rudder * delta, Vector3.up);
		}

		// Update wind and velocity
		Vector3 windForce = _sails.getSailForce();
		float desiredSpeed = windForce.magnitude / mass;
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

		if (currentSpeed > 0) {
			_velocity = transform.localRotation * Vector3.forward * currentSpeed;
			transform.position += _velocity * delta;

			IdentifySettlement();
		}

		if (_wake != null) {
			_wake.gameObject.SetActive(velocity.sqrMagnitude > 0.1f);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Settlement" || collision.gameObject.tag == "Terrain") {
			_velocity = Vector3.zero;
			SetSailState(SailState.None);
		}
	}

	private void UpdateFlag() {
		Vector3 direction = WindField.instance.GetDirectionAtPosition(transform.position);

		//if (direction != Vector3.zero) {
			_flag.localRotation = Quaternion.LookRotation(-direction, Vector3.up) * Quaternion.Inverse(transform.rotation);
		//}
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

	public void FullStop() {
		_sails.sailState = SailState.None;
		_velocity = Vector3.zero;
	}

	public void SetRudder(float rudder) {
		_rudder = rudder;
	}

	/// <summary>
	/// Identify the settlement, if any, the ship is in docking range with
	/// </summary>
	private void IdentifySettlement() {
		GameObject[] settlements = GameObject.FindGameObjectsWithTag("Settlement");

		foreach (var go in settlements) {
			Settlement settlement = go.GetComponent<Settlement>();

			if (settlement && settlement.CanDock(this)) {
				if (_currentSettlement != settlement) {
					Debug.Log("Entering " + settlement.name);
				}
				
				_currentSettlement = settlement;
				return;
			}
		}

		if (_currentSettlement != null) {
			Debug.Log("Leaving " + _currentSettlement.name);
		}

		_currentSettlement = null;
	}
}
