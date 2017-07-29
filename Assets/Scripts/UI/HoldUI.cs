using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class HoldUI : Page {

		[SerializeField]
		private Text _capacityLabel;

		[SerializeField]
		private Text _illegalCountLabel;

		[SerializeField]
		private Text _hideCountLabel;

		[SerializeField]
		private ScrollRect _scroll;

		[SerializeField]
		private GameObject _holdItem;

		[SerializeField]
		private RectTransform _listContainer;

		private CargoHold _hold;

		private List<HoldItem> _items = new List<HoldItem>();

		public void Show(CargoHold hold) {
			_hold = hold;

			UpdateDisplay();
		}

		private void UpdateDisplay() {
			_listContainer.DetachChildren();
			foreach (var item in _items) {
				Destroy(item.gameObject);
			}

			BuildList();

			_capacityLabel.text = _hold.currentCargo + "/" + _hold.capacity;
			_illegalCountLabel.text = _hold.GetIllegalCount().ToString();
			_hideCountLabel.text = _hold.GetHidingCount().ToString();
		}

		private void BuildList() {
			var manifest = _hold.manifest;
			int y = 0;
			Object resource = Resources.Load("UI/Hold Item");

			foreach (var cargo in manifest) {
				var go = Instantiate(resource) as GameObject;
				var item = go.GetComponent<HoldItem>();
				go.name = cargo.Key.ToString();

				item.transform.SetParent(_listContainer, false);
				item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
				y -= 40;

				item.SetCargo(cargo.Key, cargo.Value);
				_items.Add(item);
			}
		}
	}
}