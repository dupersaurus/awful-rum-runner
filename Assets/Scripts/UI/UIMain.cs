using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class UIMain : MonoBehaviour {
		private static UIMain _instance;

		private Page _activePage = null;

		public static bool hasFocus {
			get { return _instance._activePage != null; }
		}

		private WarehouseUI _warehouse;

		// Use this for initialization
		void Awake () {
			_instance = this;
			_warehouse = GetComponentInChildren<WarehouseUI>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Close() {
			if (_activePage) {
				_activePage.Hide();
			}

			_activePage = null;
		}

		public static void OpenWarehouse(SettlementService.Warehouse warehouse) {
			_instance._warehouse.Show(GameState.hold, warehouse, GameState.assets);
		}
	}
}
