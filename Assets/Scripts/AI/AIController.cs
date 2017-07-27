using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController {

	private bool _initialized = false;

	[SerializeField]
	private AIState _initialState;

	[SerializeField]
	private AIState _currentState;

	private bool _initNewState = false;

	private UI.WorldSpaceFloater _shipUI;

	private Dictionary<string, RectTransform> _upperIcons = new Dictionary<string, RectTransform>();

	void Awake() {
		ChangeToState<WaitState>();
	}

	void Update() {
		if (!_initialized) {
			return;
		}

		if (_initNewState) {
			_currentState.Enable();
			_initNewState = false;
		}
	}

	public void Initialize() {
		_shipUI = UI.UIMain.CreateEmptyFloater(transform);
		_initialized = true;
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

	public RectTransform AddIcon(string type) {
		if (_upperIcons.ContainsKey(type)) {
			return null;
		}

		RectTransform icon = UI.UIMain.AddUIIcon(type, _shipUI.GetComponent<RectTransform>());
		_upperIcons.Add(type, icon);

		PositionUpperIcons();

		return icon;
	}

	public void RemoveIcon(string type) {
		if (!_upperIcons.ContainsKey(type)) {
			return;
		}

		Destroy(_upperIcons[type].gameObject);
		_upperIcons.Remove(type);

		PositionUpperIcons();
	}

	public void ShowFactionFlag() {
		AddIcon(FactionFlags.GetFlagIconName(GetComponent<Ship>().faction));
	}

	public void HideFactionFlag() {
		RemoveIcon(FactionFlags.GetFlagIconName(GetComponent<Ship>().faction));
	}

	private void PositionUpperIcons() {
		float height = 42;
		float x = 0;
		float y = 0;

		foreach (var icon in _upperIcons) {
			var pos = new Vector3(x, y, 0);
			icon.Value.localPosition = pos;

			y += height;
		}
	}
}
