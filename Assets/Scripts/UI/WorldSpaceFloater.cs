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
			var pos = _track.position;
			pos.y += _verticalOffset;
			pos = Camera.main.WorldToViewportPoint(pos);

			if (pos.x <= PADDING) {
				pos.x = PADDING;
			} else if (pos.x >= 1 - PADDING) {
				pos.x = 1 - PADDING;
			}

			_rect.anchorMin = pos;
			_rect.anchorMax = pos;
		}
	}
}
