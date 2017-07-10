using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettlementService {
	[System.Serializable]
	public class Ware {
		public string id;
		public float priceMod;
		public int count;
	}

	public class Warehouse : MonoBehaviour {

		/// <summary>
		/// List of cargos the settlement wants, with a scalar representing the price markup
		/// </summary>
		[SerializeField]
		private Ware[] _wants;

		public Ware[] buyList {
			get { return _wants; }
		}

		/// <summary>
		/// List of cargos the settlement sells, with a scalar representing the markup from the base price
		/// </summary>
		[SerializeField]
		private Ware[] _sells;

		public Ware[] sellList {
			get { return _sells; }
		}

		[SerializeField]
		private int _cash;

		public int cash {
			get { return _cash; }
		}

		public Settlement settlement {
			get { return GetComponent<Settlement>(); }
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}