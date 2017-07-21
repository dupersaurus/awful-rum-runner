using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class CompassUI : MonoBehaviour {

		[SerializeField]
		private RectTransform _compass;

		[SerializeField]
		private RectTransform[] _counterRotators;

		[SerializeField]
		private RectTransform _windGauge;

		// Update is called once per frame
		void Update () {
			Ship player = GameState.playerShip;

			// Compass
			float heading = Vector3.Angle(Vector3.forward, player.transform.rotation * Vector3.forward);
			
			if (Vector3.Cross(Vector3.forward, player.transform.rotation * Vector3.forward).y < 0) {
				heading = 360 - heading;
			}

			Quaternion rot = Quaternion.AngleAxis(heading, Vector3.forward);
			Quaternion inv = Quaternion.AngleAxis(-heading, Vector3.forward);
			
			_compass.rotation = rot;

			foreach (var item in _counterRotators) {
				item.localRotation = inv;
			}

			// Wind
			var wind = WindField.instance.GetDirectionAtPosition(player.position);
			float windAngle = Vector3.Angle(Vector3.forward, wind) + 180;

			if (Vector3.Cross(Vector3.forward, wind).y >= 0) {
				windAngle = 360 - windAngle;
			}

			_windGauge.localRotation = Quaternion.AngleAxis(windAngle, Vector3.forward);
		}
	}
}