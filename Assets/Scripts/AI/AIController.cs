using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController {

	[SerializeField]
	private AIState _initialState;

	void Start() {
		if (_initialState) {
			_initialState.Enable();
		}
	}
}
