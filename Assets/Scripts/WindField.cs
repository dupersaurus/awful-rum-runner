using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour {

	[System.Serializable]
	public class WindZone {
		public string name;
		public Color color;
		public Bounds bounds;
		public float direction;
		public float strength; 
	}

	private static WindField _instance;

	[SerializeField]
	private WindZone[] _zones;

	void Awake () {
		_instance = this;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;

		if (_zones != null) {
			foreach (var zone in _zones) {
				Gizmos.color = zone.color;
				Gizmos.DrawWireCube(zone.bounds.center, zone.bounds.size);

				Vector3 pos = zone.bounds.center;
				pos.y += 30;
				Vector3 to = pos;

				to += (Quaternion.AngleAxis(180 + zone.direction, Vector3.up) * Vector3.forward) * 20;

				Gizmos.DrawLine(pos, to);
				Gizmos.DrawWireSphere(pos, 5);
			}
		}
	}

	public static WindField instance {
		 get { return _instance; }
	}

	/// <summary>
	/// Returns the direction of the wind at a given position
	/// </summary>
	/// <param name="pos">The position to check at</param>
	/// <returns>
	/// The direction the wind is coming from
	/// </returns>
	public Vector3 GetDirectionAtPosition(Vector2 pos) {
		return Vector3.zero;
	}

	public Vector3 GetDirectionAtPosition(Vector3 pos) {
		if (_zones == null || _zones.Length == 0) {
			return Quaternion.AngleAxis(180 + 135, Vector3.up) * Vector3.forward;
		}

		List<WindZone> hits = new List<WindZone>();
		Vector3 net = new Vector3();

		foreach (var zone in _zones) {
			if (zone.bounds.Contains(pos)) {
				hits.Add(zone);
			}
		}

		foreach (var hit in hits) {
			var dir = Quaternion.AngleAxis(180 + hit.direction, Vector3.up) * Vector3.forward;
			net += dir;
		}

		net /= hits.Count;

		return net;
	}

	/// <summary>
	/// The speed of the wind at a given position
	/// </summary>
	/// <param name="pos">The position to check at</param>
	/// <returns>The speed of the wind</returns>
	public int GetSpeedAtPosition(Vector2 pos) {
		return 10;
	}
}
