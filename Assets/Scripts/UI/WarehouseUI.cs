using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SettlementService;

namespace UI {
	public class WarehouseUI : Page {
		[SerializeField]
		private RectTransform _listPanel;

		[SerializeField]
		private UnityEngine.UI.Text _settlementName;

		[SerializeField]
		private UnityEngine.UI.Button _sellingButton;

		[SerializeField]
		private UnityEngine.UI.Button _buyingButton;

		[SerializeField]
		private UnityEngine.UI.Button _buyButton;

		[SerializeField]
		private UnityEngine.UI.Button _sellButton;

		[SerializeField]
		private UnityEngine.UI.Text _playerGoldLabel;

		[SerializeField]
		private UnityEngine.UI.Text _sellerGoldLabel;

		[SerializeField]
		private UnityEngine.UI.Text _selectedWareNameLabel;

		[SerializeField]
		private UnityEngine.UI.Text _playerHoldLabel;

		[SerializeField]
		private UnityEngine.UI.Text _sellerHoldLabel;

		[SerializeField]
		private UnityEngine.UI.Text _playerCapacityLabel;

		[SerializeField]
		private UnityEngine.UI.Text _transactionModeLabel;

		private Object _listItemPrefab;

		private CargoHold _ship;
		private Warehouse _warehouse;

		private List<PurchaseItem> _itemList = new List<PurchaseItem>();

		private int _selectedItem = 0;

		void Update() {
			if (Input.GetButtonDown("Select Up")) {
				SelectUp();
			} else if (Input.GetButtonDown("Select Down")) {
				SelectDown();
			}
		}

		public void Show(CargoHold ship, Warehouse warehouse, PlayerAssets assets) {
			base.Show();

			if (_listItemPrefab == null) {
				_listItemPrefab = Resources.Load("UI/Purchase Item");
			}

			_ship = ship;
			_warehouse = warehouse;

			_settlementName.text = warehouse.settlement.name;

			UpdateMoneyCount();
			UpdateHoldCount();
			ShowSellScreen();
		}

		public void ShowSellScreen() {
			ClearItems();

			_sellButton.gameObject.SetActive(false);
			_buyButton.gameObject.SetActive(true);

			foreach (var item in _warehouse.sellList) {
				GetItemForWare(item, _itemList, _listPanel);
			}

			_transactionModeLabel.text = "Selling";
			SelectItem(0);
		}

		public void ShowBuyScreen() {
			ClearItems();

			_sellButton.gameObject.SetActive(true);
			_buyButton.gameObject.SetActive(false);

			foreach (var item in _warehouse.buyList) {
				GetItemForWare(item, _itemList, _listPanel);
			}

			_transactionModeLabel.text = "Buying";
			SelectItem(0);
		}

		private void SelectItem(int index) {
			if (index < 0) {
				index = _itemList.Count - 1;
			} else if (index >= _itemList.Count) {
				index = 0;
			}

			if (_selectedItem < _itemList.Count) {
				_itemList[_selectedItem].Deselect();
			}

			_selectedItem = index;
			_itemList[_selectedItem].Select();	

			UpdateSelectedItem();
		}

		private PurchaseItem GetItemForWare(Ware ware, List<PurchaseItem> list, RectTransform panel) {
			foreach (var item in list) {
				if (item.ware.id == ware.id) {
					return item;
				}
			}

			PurchaseItem newItem = AddItem(panel, list);
			newItem.SetWare(ware, _warehouse);
			return newItem;
		}

		private PurchaseItem AddItem(RectTransform parent, List<PurchaseItem> list) {
			GameObject go = Instantiate(_listItemPrefab) as GameObject;
			RectTransform gorect = go.GetComponent<RectTransform>();
			PurchaseItem uiItem = go.GetComponent<PurchaseItem>();

			float yPos = -5 - list.Count * uiItem.height;

			go.transform.SetParent(parent, false);
			gorect.anchoredPosition = new Vector2(10, yPos);
			uiItem.Deselect();

			yPos += uiItem.height;
			list.Add(uiItem);

			return uiItem;
		}

		private void ClearItems() {
			_listPanel.DetachChildren();

			foreach (var item in _itemList) {
				DestroyImmediate(item.gameObject);	
			}

			_itemList.Clear();
		}

		public override void Hide() {
			base.Hide();
		}

		/// <summary>
		/// Buy an amount of the selected item from the merchant
		/// </summary>
		/// <param name="amount">The amount to buy</param>
		public void BuyItem(int amount = 1) {
			if (LedgerManager.ProcessBuy(_ship, _warehouse, GameState.assets, _itemList[_selectedItem].ware.id, amount)) {
				UpdateDisplay();
			}
		}

		/// <summary>
		/// Sell an amount of the selected item to the merchant
		/// </summary>
		/// <param name="amount">The amount to sell</param>
		public void SellItem(int amount = 1) {
			if (LedgerManager.ProcessSell(_ship, _warehouse, GameState.assets, _itemList[_selectedItem].ware.id, amount)) {
				UpdateDisplay();
			}
		}

		private void UpdateDisplay() {
			UpdateMoneyCount();
			UpdateHoldCount();
			UpdateSelectedItem();
		}

		private void UpdateMoneyCount() {
			_playerGoldLabel.text = GameState.assets.cash.ToString();
			_sellerGoldLabel.text = _warehouse.cash.ToString();
		}

		private void UpdateHoldCount() {
			_playerCapacityLabel.text = (_ship.capacity - _ship.currentCargo).ToString();

			foreach (var item in _itemList) {
				item.RefreshDisplay();
			}
		}

		private void UpdateSelectedItem() {
			if (_selectedItem < _itemList.Count) {
				PurchaseItem item = _itemList[_selectedItem];
				CargoList.CargoEntry cargo = CargoManager.GetCargo(item.ware.id);
				
				_selectedWareNameLabel.text = cargo.name;
				_sellerHoldLabel.text = item.ware.count.ToString();
				_playerHoldLabel.text = _ship.GetItemCount(cargo.id).ToString();
			}
		}

		private void SelectUp() {
			SelectItem(_selectedItem - 1);
		}

		private void SelectDown() {
			SelectItem(_selectedItem + 1);
		}
	}
}
