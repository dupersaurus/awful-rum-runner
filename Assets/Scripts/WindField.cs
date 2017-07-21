using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField {

	private static WindField _instance;

	public WindField () {
		_instance = this;
	}
	
	// Update is called once per frame
	public void Update (float delta) {
		
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
		return new Vector3(-1, 0, 1).normalized;
	}

	public Vector3 GetDirectionAtPosition(Vector3 pos) {
		return new Vector3(-1, 0, 1).normalized;
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
