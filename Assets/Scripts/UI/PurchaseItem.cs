using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class PurchaseItem : MonoBehaviour {

		private SettlementService.Ware _ware;
		private SettlementService.Warehouse _warehouse;

		public SettlementService.Ware ware {
			get { return _ware; }
			set {
				_ware = value;

				var cargo = CargoManager.GetCargo(_ware.id);

				_nameLabel.text = cargo.name;
				_countLabel.text = "x" + _ware.count;
				_priceLabel.text = "$" + _ware.priceMod * cargo.price;
			}
		}

		[SerializeField]
		private Text _nameLabel;

		[SerializeField]
		private Text _countLabel;

		[SerializeField]
		private Text _priceLabel;

		[SerializeField]
		private float _height;

		[SerializeField]
		private RectTransform _selectedIcon;

		public float height {
			get {
				return _height;
			}
		}

		// Use this for initialization
		void Awake () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Select() {
			_selectedIcon.gameObject.SetActive(true);
		}

		public void Deselect() {
			_selectedIcon.gameObject.SetActive(false);
		}

		public void SetWare(SettlementService.Ware ware, SettlementService.Warehouse warehouse) {
			_ware = ware;
			_warehouse = warehouse;

			RefreshDisplay();
		}

		public void RefreshDisplay() {
			var cargo = CargoManager.GetCargo(_ware.id);

			_nameLabel.text = cargo.name;
			_countLabel.text = "x" + _ware.count;
			_priceLabel.text = _warehouse.GetPrice(_ware.id, 1) + "G";
		}
	}
}
