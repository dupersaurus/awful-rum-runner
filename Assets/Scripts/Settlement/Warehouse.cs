using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettlementService {
	[System.Serializable]
	public class Ware {
		public string id;
		public float priceMod;
	}

	public class Warehouse : MonoBehaviour {

		/// <summary>
		/// List of cargos the settlement wants, with a scalar representing the price markup
		/// </summary>
		[SerializeField]
		private Ware[] _wants;

		/// <summary>
		/// List of cargos the settlement sells, with a scalar representing the markup from the base price
		/// </summary>
		[SerializeField]
		private Ware[] _sells;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}