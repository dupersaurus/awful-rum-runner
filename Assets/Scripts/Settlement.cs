using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SettlementService;

public enum Faction {
	Pirate,
	Islander,
	Mainlander,
	Empire
}

public class Settlement : MonoBehaviour {

	/// <summary>
	/// Name of the settlement
	/// </summary>
	[SerializeField]
	private string _name;

	/// <summary>
	/// The faction the settlement belongs to
	/// </summary>
	[SerializeField]
	private Faction _faction;

	[SerializeField]
	private Bounds _dockArea;

	private Warehouse _warehouse;

	public Warehouse warehouse {
		get { return _warehouse; }
	}

	// Use this for initialization
	void Awake () {
		_warehouse = GetComponent<Warehouse>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(transform.position + _dockArea.center, _dockArea.size);
	}

	/// <summary>
	/// Returns whether a ship can dock at the settlement at it's present position
	/// </summary>
	/// <param name="ship">The ship to check</param>
	/// <returns>Whether the ship can dock or not</returns>
	public bool CanDock(Ship ship) {
		Vector3 shipPos = ship.transform.position - transform.position;
		return _dockArea.Contains(shipPos);
	}
}
