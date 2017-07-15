using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterCrew : Crew {

	/// <summary>
	/// Skill at finding illicit cargo
	/// </summary>
	[SerializeField]
	private int _inspectionSkill = 25;

	public int inspectionSkill {
		get { return _inspectionSkill; }
	}

	/// <summary>
	/// Resistance to bribing
	/// </summary>
	[SerializeField]
	private int _bribeSkill = 25;

	public int bribeSkill {
		get { return _bribeSkill; }
	}
}
