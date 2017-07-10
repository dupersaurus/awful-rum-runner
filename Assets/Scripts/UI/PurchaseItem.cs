using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class PurchaseItem : MonoBehaviour {

		private SettlementService.Ware _ware;

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

		public float height {
			get {
				return _height;
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Select() {

		}

		public void Deselect() {
			
		}
	}
}
