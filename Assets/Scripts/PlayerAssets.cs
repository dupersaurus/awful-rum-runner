using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player's personal fortune
/// </summary>
public class PlayerAssets {

	/// <summary>
	/// Cash on hand
	/// </summary>
	private int _cash = 100;

	/// <summary>
	/// Cash in each bank in the world
	/// </summary>
	private Dictionary<string, int> _bank = new Dictionary<string, int>();

	/// <summary>
	/// Debts owed to each bank in the world
	/// </summary>
	private Dictionary<string, int> _debts = new Dictionary<string, int>();

	public int cash {
		get { return _cash; }
	}
}
