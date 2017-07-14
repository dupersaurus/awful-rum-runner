using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterCrew : MonoBehaviour {

	/// <summary>
	/// Skill at finding illicit cargo
	/// </summary>
	[SerializeField]
	private int _inspectionSkill = 25;

	/// <summary>
	/// Resistance to bribing
	/// </summary>
	[SerializeField]
	private int _bribeSkill = 25;
}
