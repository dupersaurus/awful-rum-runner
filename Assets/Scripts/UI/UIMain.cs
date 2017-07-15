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
			//_warehouse = GetComponentInChildren<WarehouseUI>();

			//_warehouse.ui = this;
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Close() {
			if (_activePage) {
				_activePage.Hide();
			}

			GameState.ReleaseGlobalPause(_activePage);
			_activePage = null;
		}

		protected Page OpenScreen(string prefab) {
			GameObject go = Instantiate(Resources.Load("UI/" + prefab)) as GameObject;
			go.transform.SetParent(GetComponent<RectTransform>(), false);

			RectTransform gorect = go.GetComponent<RectTransform>();
			Page page = go.GetComponent<Page>();

			page.ui = this;
			_activePage = page;

			return page;
		}

		public static void OpenWarehouse(SettlementService.Warehouse warehouse) {
			WarehouseUI page = _instance.OpenScreen("Settlement Wares") as WarehouseUI;
			page.Show(GameState.hold, warehouse, GameState.assets);

			GameState.SetGlobalPause(page);
		}

		public static void OpenBoardingAction() {
			BoardingUI page = _instance.OpenScreen("Boarding Action") as BoardingUI;
			page.Begin();

			GameState.SetGlobalPause(page);
		}
	}
}
