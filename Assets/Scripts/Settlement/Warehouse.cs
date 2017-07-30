using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettlementService {
	[System.Serializable]
	public class Ware {
		public Cargoes id;
		public float priceMod;
		public int count;

		public Ware Clone() {
			return new Ware{id = id, priceMod = priceMod, count = count};
		}
	}

	public class Warehouse : MonoBehaviour, ISettlementService {

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

		private List<Ware> _resetWants;
		private List<Ware> _resetSells;
		private int _resetCash;

		public Settlement settlement {
			get { return GetComponent<Settlement>(); }
		}

		void Awake() {
			_resetCash = _cash;
			_resetWants = new List<Ware>();
			_resetSells = new List<Ware>();

			for (int i = 0; i < _wants.Length; i++) {
				_resetWants.Add(_wants[i].Clone());
			}

			for (int i = 0; i < _sells.Length; i++) {
				_resetSells.Add(_sells[i].Clone());
			}
		}

		// Use this for initialization
		public void Initialize() {
			
		}

		public void Reset() {
			_cash = _resetCash;

			var wants = new List<Ware>();
			var sells = new List<Ware>();
			
			for (int i = 0; i < _resetWants.Count; i++) {
				wants.Add(_resetWants[i].Clone());
			}

			for (int i = 0; i < _resetSells.Count; i++) {
				sells.Add(_resetSells[i].Clone());
			}

			_wants = wants.ToArray();
			_sells = sells.ToArray();
		}

		/// <summary>
		/// Modify the quantity of a ware being sold
		/// </summary>
		/// <param name="id">The id of the ware</param>
		/// <param name="amount">The amount to change, positive or negative</param>
		public void ModifySellingWare(Cargoes id, int amount) {
			Ware ware = GetSellWare(id);

			if (ware != null) {
				ware.count += amount;

				if (ware.count < 0) {
					ware.count = 0;
				}
			}
		}

		/// <summary>
		/// Modify the quantity of a ware being bought
		/// </summary>
		/// <param name="id">The id of the ware</param>
		/// <param name="amount">The amount to change, positive or negative</param>
		public void ModifyBuyingWare(Cargoes id, int amount) {
			Ware ware = GetBuyWare(id);

			if (ware != null) {
				ware.count += amount;

				if (ware.count < 0) {
					ware.count = 0;
				}
			}
		}

		public void ModifyCash(int amount) {
			_cash += amount;

			if (_cash < 0) {
				_cash = 0;
			}
		}

		/// <summary>
		/// Returns the price the warehouse sells a cargo for
		/// </summary>
		/// <param name="id">Id of the cargo to sell</param>
		/// <param name="count">Optional cargo count</param>
		/// <returns>The selling price</returns>
		public int GetSellPrice(Cargoes id, int count = 1) {
			Ware ware = GetSellWare(id);

			if (ware == null) {
				return 0;
			}

			int basePrice = Mathf.RoundToInt(ware.priceMod * CargoManager.GetCargo(id).price);

			return basePrice * count;
		}

		/// <summary>
		/// Returns the price the warehouse buys a cargo for
		/// </summary>
		/// <param name="id">Id of the cargo to buy</param>
		/// <param name="count">Optional cargo count</param>
		/// <returns>The buying price</returns>
		public int GetBuyPrice(Cargoes id, int count = 1) {
			Ware ware = GetBuyWare(id);

			if (ware == null) {
				return 0;
			}

			int basePrice = Mathf.RoundToInt(ware.priceMod * CargoManager.GetCargo(id).price);

			return basePrice * count;
		}

		public int GetPrice(Cargoes id, int count = 1) {
			if (GetSellWare(id) != null) {
				return GetSellPrice(id, count);
			} else if (GetBuyWare(id) != null) {
				return GetBuyPrice(id, count);
			}
			
			return 0;
		}

		private Ware GetSellWare(Cargoes id) {
			foreach (var ware in _sells) {
				if (ware.id == id) {
					return ware;
				}
			}

			return null;
		}

		private Ware GetBuyWare(Cargoes id) {
			foreach (var ware in _wants) {
				if (ware.id == id) {
					return ware;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the amount of a cargo the settlement has to sell
		/// </summary>
		/// <param name="id">The id of the cargo</param>
		/// <returns>The quatity of cargo at the merchant</returns>
		public int GetWareQuantity(Cargoes id) {
			Ware ware = GetSellWare(id);

			if (ware == null) {
				return 0;
			}

			return ware.count;
		}

		/// <summary>
		/// Returns the amount of a cargo the settlement wants to buy
		/// </summary>
		/// <param name="id">The id of the cargo</param>
		/// <returns>The quatity of cargo to buy</returns>
		public int GetWantQuantity(Cargoes id) {
			Ware ware = GetBuyWare(id);

			if (ware == null) {
				return 0;
			}

			return ware.count;
		}

		public bool IsBlockaded() {
			return GetComponent<Settlement>().faction == Faction.Mainlander;
		}
	}
}