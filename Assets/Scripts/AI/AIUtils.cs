using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUtils {

	/// <summary>
	/// Returns if a ship going a certain direction would be in the no-go zone
	/// </summary>
	/// <param name="ship">The ship in question</param>
	/// <param name="pos">The position of the ship</param>
	/// <param name="direction">The direction the ship is heading</param>
	/// <returns>whether the ship would be in the no-go zone</returns>
	public static bool IsInNoGo(Ship ship, Vector3 pos, Vector3 direction) {
		return true;
	}

	/// <summary>
	/// Get the best tack vector on the other side of the wind
	/// </summary>
	/// <param name="ship">The ship</param>
	/// <param name="pos">The ship's position</param>
	/// <param name="direction">The ship's current heading</param>
	/// <returns></returns>
	public static Vector3 GetTackVector(Ship ship, Vector3 pos, Vector3 direction) {
		return Vector3.zero;
	}	
}
