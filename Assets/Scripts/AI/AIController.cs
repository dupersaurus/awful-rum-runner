using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController {

	[SerializeField]
	private AIState _initialState;

	[SerializeField]
	private AIState _currentState;

	private bool _initNewState = false;

	void Start() {
		if (_initialState) {
			ActivateState(_initialState);
		}
	}

	void Update() {
		if (_initNewState) {
			_currentState.Enable();
			_initNewState = false;
		}
	}

	public T ChangeToState<T>() where T : AIState {
		T state = GetComponent<T>();

		if (state == null) {
			state = gameObject.AddComponent<T>();
		}

		ActivateState(state);

		return state;
	}

	private void ActivateState(AIState state) {
		if (_currentState != null) {
			_currentState.Disable();
		}

		_currentState = state;
		_initNewState = true;
	}
}
