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
	public static bool ProcessBuy(CargoHold ship, Warehouse warehouse, PlayerAssets assets, Cargoes id, int count) {
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

	/// <summary>
	/// Process selling cargo to a merchant
	/// </summary>
	/// <param name="ship">The ship doing the selling</param>
	/// <param name="warehouse">The merchant doing the buying</param>
	/// <param name="assets">The player's wealth</param>
	/// <param name="id">The cargo to sell</param>
	/// <param name="count">The amount to sell</param>
	/// <returns>Successfulness</returns>
	public static bool ProcessSell(CargoHold ship, Warehouse warehouse, PlayerAssets assets, Cargoes id, int count) {
		int price = warehouse.GetBuyPrice(id, count);

		if (warehouse.cash < price) {
			return false;
		}

		if (warehouse.GetWantQuantity(id) == 0) {
			return false;
		}

		if (ship.GetItemCount(id) < count) {
			return false;
		}

		ship.Remove(id, count);
		warehouse.ModifyBuyingWare(id, -count);
		
		assets.ModifyCash(price);
		warehouse.ModifyCash(-price);

		return true;
	}
}
