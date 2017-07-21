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

			OpenHUD();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Close() {
			ClearScreen();
			OpenHUD();
		}

		protected void ClearScreen() {
			if (_activePage) {
				_activePage.Hide();
				GameState.ReleaseGlobalPause(_activePage.name);
				
				Destroy(_activePage);
			}
			_activePage = null;
		}

		protected Page OpenScreen(string prefab) {
			if (_activePage != null) {
				ClearScreen();
			}

			GameObject go = Instantiate(Resources.Load("UI/" + prefab)) as GameObject;
			go.transform.SetParent(GetComponent<RectTransform>(), false);

			RectTransform gorect = go.GetComponent<RectTransform>();
			Page page = go.GetComponent<Page>();

			page.ui = this;
			_activePage = page;

			return page;
		}

		protected void OpenHUD() {
			_instance.OpenScreen("HUD");
		}

		public static void OpenWarehouse(SettlementService.Warehouse warehouse) {
			WarehouseUI page = _instance.OpenScreen("Settlement Wares") as WarehouseUI;
			page.Show(GameState.hold, warehouse, GameState.assets);

			GameState.SetGlobalPause(page.name);
		}

		public static void OpenBoardingAction() {
			BoardingUI page = _instance.OpenScreen("Boarding Action") as BoardingUI;
			page.Begin();

			GameState.SetGlobalPause(page.name);
		}

		public static ResolveBoardingUI OpenResolveBoarding() {
			ResolveBoardingUI page = _instance.OpenScreen("Resolve Boarding") as ResolveBoardingUI;
			page.Begin();

			GameState.SetGlobalPause(page.name);

			return page;
		}

		public static void OpenPayFine(BoardingManager manager) {
			PayFineUI page = _instance.OpenScreen("Fine") as PayFineUI;
			page.Begin(manager);

			GameState.SetGlobalPause(page.name);
		}

		public static WorldSpaceFloater DemandBoarding(Transform target) {
			var floaters = _instance.GetComponentInChildren<FloatingIcons>();
			var icon = floaters.AddWorldFloater("Submit Boarding Icon", target);
			icon.offset = 0.8f;

			return icon;
		}

		public static TimeToBoardFloater AddTimeToBoard(Transform target) {
			var floaters = _instance.GetComponentInChildren<FloatingIcons>();
			var icon = floaters.AddWorldFloater("Time To Board", target);
			icon.offset = 0.8f;

			return icon as TimeToBoardFloater;
		}
	}
}
