using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SettlementService;

public class LedgerManager {
	private static LedgerManager _instance;

	protected static LedgerManager instance {
		get {
			if (_instance == null) {
				_instance = new LedgerManager();
			}

			return _instance;
		}
	}

	public LedgerManager() {
		
	}

	/// <summary>
	/// Process buying cargo from a merchant
	/// </summary>
	/// <param name="ship">The ship doing the buying</param>
	/// <param name="warehouse">The merchant doing the selling</param>
	/// <param name="assets">The player's wealth</param>
	/// <param name="id">The cargo to buy</param>
	/// <param name="count">The amount to buy</param>
	/// <returns>Successfulness</returns>
	public static bool ProcessBuy(CargoHold ship, Warehouse warehouse, PlayerAssets assets, string id, int count) {
		int price = warehouse.GetSellPrice(id, count);

		if (assets.cash < price) {
			return false;
		}

		if (warehouse.GetWareQuantity(id) == 0) {
			return false;
		}

		if (ship.availableSpace < count) {
			return false;
		}

		assets.ModifyCash(-price);
		ship.Add(id, count);
		warehouse.ModifySellingWare(id, -count);
		warehouse.ModifyCash(price);

		return true;
	}
}
