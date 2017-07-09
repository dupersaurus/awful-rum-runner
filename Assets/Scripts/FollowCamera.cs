using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : UnityStandardAssets.Cameras.AbstractTargetFollower {

	[SerializeField]
	private Vector3 cameraOffset = Vector3.zero;

	[SerializeField]
	private Vector3 cameraRotOffset = Vector3.zero;

	[SerializeField]
	private float _rotationSlop = 1;

	[SerializeField]
	private float _lookSpeed = 90;

	private Ship _ship;

	private float _slerpTime = 0;

	private Quaternion _lookAngle = Quaternion.identity;

	protected override void Start() {
		base.Start();
		_ship = m_Target.GetComponent<Ship>();
	}
	protected override void FollowTarget(float deltaTime) {
		// Look around 
		float lookAround = Input.GetAxis("Horizontal Camera") * _lookSpeed * deltaTime;
		
		if (lookAround != 0) {
			_lookAngle *= Quaternion.AngleAxis(lookAround, Vector3.up);
		}

		// Where we want to be
		Vector3 wantPosition = m_Target.position + m_Target.transform.rotation * (_lookAngle * cameraOffset);
		Quaternion wantRotation = (m_Target.transform.rotation * _lookAngle) * Quaternion.Euler(cameraRotOffset);

		// Camera lag
		if (wantRotation == gameObject.transform.rotation) {
			_slerpTime = 0;
		} else {
			_slerpTime += deltaTime;
		}

		if (_slerpTime > _rotationSlop) {
			_slerpTime = _rotationSlop;
		}

		gameObject.transform.position = wantPosition;
		gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, wantRotation, _slerpTime / _rotationSlop);
	}
}