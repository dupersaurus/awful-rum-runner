using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SettlementService;

namespace UI {
	public class WarehouseUI : Page {
		[SerializeField]
		private RectTransform _sellPanel;

		[SerializeField]
		private RectTransform _buyPanel;

		private Object _listItemPrefab;

		private List<PurchaseItem> _sellItems = new List<PurchaseItem>();
		private List<PurchaseItem> _buyItems = new List<PurchaseItem>();

		public void Show(CargoHold ship, Warehouse warehouse, PlayerAssets assets) {
			base.Show();

			if (_listItemPrefab == null) {
				_listItemPrefab = Resources.Load("UI/Purchase Item");
			}

			float yPos = 20;

			foreach (var item in warehouse.sellList) {
				GetItemForWare(item, _sellItems, _sellPanel);
			}

			foreach (var item in warehouse.sellList) {
				GetItemForWare(item, _buyItems, _buyPanel);
			}
		}

		private PurchaseItem GetItemForWare(Ware ware, List<PurchaseItem> list, RectTransform panel) {
			foreach (var item in list) {
				if (item.ware.id == ware.id) {
					return item;
				}
			}

			PurchaseItem newItem = AddItem(panel, list);
			newItem.ware = ware;
			return newItem;
		}

		private PurchaseItem AddItem(RectTransform parent, List<PurchaseItem> list) {
			GameObject go = Instantiate(_listItemPrefab) as GameObject;
			RectTransform gorect = go.GetComponent<RectTransform>();
			PurchaseItem uiItem = go.GetComponent<PurchaseItem>();

			float yPos = -10 - list.Count * uiItem.height;

			go.transform.SetParent(_sellPanel, false);
			gorect.anchoredPosition = new Vector2(10, yPos);

			yPos += uiItem.height;
			_sellItems.Add(uiItem);

			return uiItem;
		}

		public override void Hide() {
			base.Hide();
		}
	}
}
