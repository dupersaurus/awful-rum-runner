using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class SplashScreen : Page {

		[SerializeField]
		private RectTransform _credits;

		void Awake() {
			_credits.gameObject.SetActive(false);
		}

		public void ToggleCredits() {
			_credits.gameObject.SetActive(!_credits.gameObject.activeInHierarchy);
		}

		public void StartGame() {
			Close();
			GameState.StartGame();
		}
	}
}