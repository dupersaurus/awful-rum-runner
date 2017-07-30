using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class UIMain : MonoBehaviour {
		private static UIMain _instance;

		private Page _activePage = null;
		private Page _hudPage = null;

		public static bool hasFocus {
			get { return _instance._activePage != null; }
		}

		private WarehouseUI _warehouse;

		public static int screenHeight {
			get { return Screen.height; }
		}

		// Use this for initialization
		void Awake () {
			_instance = this;
			
			//OpenHUD();
			OpenScreen("Splash Screen");
		}

		public static void CloseScreen() {
			_instance.Close();
		}

		public void Close() {
			ClearScreen();
			OpenHUD();
		}

		protected void ClearScreen() {
			if (_activePage) {
				_activePage.Hide();
				GameState.ReleaseGlobalPause(_activePage.name);
				
				if (_activePage != _hudPage) {
					Destroy(_activePage.gameObject);
				}
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
			if (_hudPage == null) {
				_hudPage = _instance.OpenScreen("HUD");
			} else {
				_hudPage.gameObject.SetActive(true);
				_activePage = _hudPage;
			}
		}

		public static void Reset() {
			_instance.OpenScreen("Splash Screen");
		}

		public static void OpenWarehouse(SettlementService.Warehouse warehouse) {
			WarehouseUI page = _instance.OpenScreen("Settlement Wares") as WarehouseUI;
			page.Show(GameState.hold, warehouse, GameState.assets);

			GameState.SetGlobalPause(page.name);
		}

		public static void OpenBank(SettlementService.Bank bank) {
			BankUI page = _instance.OpenScreen("Bank Screen") as BankUI;
			page.Show(bank, GameState.assets);

			GameState.SetGlobalPause(page.name);
		}

		public static void OpenCargoHold(CargoHold hold) {
			HoldUI page = _instance.OpenScreen("Hold Screen") as HoldUI;
			page.Show(hold);

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

		public static void OpenArrestedStory() {
			Page page = _instance.OpenScreen("Endings/Arrested");
			GameState.SetGlobalPause(page.name);
		}

		public static void OpenContinueStory() {
			Page page = _instance.OpenScreen("Endings/Continue");
			GameState.SetGlobalPause(page.name);
		}

		public static void OpenGameOverStory() {
			Page page = _instance.OpenScreen("Endings/Game Over");
			GameState.SetGlobalPause(page.name);
		}

		public static WorldSpaceFloater CreateEmptyFloater(Transform target) {
			return CreateEmptyFloater(target, 0.8f);
		}

		public static WorldSpaceFloater CreateEmptyFloater(Transform target, float offset) {
			var floaters = _instance.GetComponentInChildren<FloatingIcons>();
			var icon = floaters.AddWorldFloater("Empty Floater", target);
			icon.offset = offset;
			icon.gameObject.name = target.gameObject.name;

			return icon;
		}

		public static RectTransform AddUIIcon(string type, RectTransform parent) {
			GameObject go = Instantiate(Resources.Load("UI/" + type)) as GameObject;
			RectTransform f = go.GetComponent<RectTransform>();

			if (f) {
				go.transform.SetParent(parent, false);
				return f;
			} else {
				DestroyImmediate(go);
				return null;
			}
		}

		public static TimeToBoardFloater AddTimeToBoard(Transform target) {
			var floaters = _instance.GetComponentInChildren<FloatingIcons>();
			var icon = floaters.AddWorldFloater("Time To Board", target);
			icon.offset = -0.2f;

			return icon as TimeToBoardFloater;
		}

		public static void DestroyFloater(WorldSpaceFloater go) {
			Destroy(go.gameObject);
		}
	}
}
