using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoManager {

	private static CargoManager _instance;

	private Dictionary <Cargoes, CargoList.CargoEntry> _cargos = new Dictionary<Cargoes, CargoList.CargoEntry>();

	public CargoManager() {
		_instance = this;

		TextAsset cargoDef = Resources.Load("Data/cargos") as TextAsset;
		CargoList cargos = JsonUtility.FromJson<CargoList>(cargoDef.text);

		foreach (var cargo in cargos.cargos) {
			_cargos.Add((Cargoes)System.Enum.Parse(typeof(Cargoes), cargo.id), cargo);
			
		}
	}

	public static CargoList.CargoEntry GetCargo(Cargoes id) {
		return _instance._cargos[id];
	}
}

[System.Serializable]
public struct CargoList {

	[System.Serializable]
	public struct CargoEntry {
		public string name;
		public string description;
		public bool legal;
		public int price;
		public string id;
		public int hideRatio;
		public Cargoes cargo {
			get { return (Cargoes)System.Enum.Parse(typeof(Cargoes), id); }
		}
	}

	public CargoEntry[] cargos;
}

public enum Cargoes {
	apples,
	cider,
	gin,
	grain,
	grog,
	hooch,
	lumber,
	potato,
	scotch,
	sugar,
	tequila,
	vodka
}