using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

	/// <summary>
	/// An icon that tracks a world object on screen space
	/// </summary>
	public class WorldSpaceFloater : MonoBehaviour {

		private const float PADDING = 0.05f;

		private RectTransform _rect;

		protected Transform _track;

		protected float _verticalOffset = 0;

		private Dictionary<string, RectTransform> _icon = new Dictionary<string, RectTransform>();

		public Transform track {
			set { _track = value; }
		}

		public float offset {
			set { _verticalOffset = value; }
		}

		void Awake() {
			_rect = GetComponent<RectTransform>();
		}

		// Update is called once per frame
		void Update () {
			Vector3 pos = Vector3.zero;

			try {
				pos = _track.position;
			} catch {
				Debug.Log("UHOH");
			}

			pos.y += _verticalOffset;
			pos = Camera.main.WorldToViewportPoint(pos);

			if (pos.z > 0) {
				if (pos.x <= PADDING) {
					pos.x = PADDING;
					pos.y = 0.6f;
				} else if (pos.x >= 1 - PADDING) {
					pos.x = 1 - PADDING;
					pos.y = 0.6f;
				}
			} else {
				if (pos.x >= 0.5) {
					pos.x = PADDING;
				} else {
					pos.x = 1 - PADDING;
				}
					
				pos.y = 0.6f;
			}

			_rect.anchorMin = pos;
			_rect.anchorMax = pos;
		}

		public RectTransform AddIcon(string type) {
			if (_icon.ContainsKey(type)) {
				return null;
			}

			RectTransform icon = UI.UIMain.AddUIIcon(type, GetComponent<RectTransform>());
			_icon.Add(type, icon);

			PositionIcons();

			return icon;
		}

		public void RemoveIcon(string type) {
			if (!_icon.ContainsKey(type)) {
				return;
			}

			Destroy(_icon[type].gameObject);
			_icon.Remove(type);

			PositionIcons();
		}

		public RectTransform ToggleIcon(string type, bool show) {
			if (show) {
				return AddIcon(type);
			} else {
				RemoveIcon(type);
			}

			return null;
		}

		private void PositionIcons() {
			float height = 42;
			float x = 0;
			float y = 0;

			foreach (var icon in _icon) {
				var pos = new Vector3(x, y, 0);
				icon.Value.localPosition = pos;

				y += height;
			}
		}
	}
}
