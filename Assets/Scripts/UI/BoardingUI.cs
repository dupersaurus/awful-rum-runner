using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class BoardingUI : Page {

		[SerializeField]
		private RectTransform _oddsBar;

		[SerializeField]
		private RectTransform _friendBar;
		
		[SerializeField]
		private RectTransform _foeBar;

		[SerializeField]
		private Text _oddsLabel;

		[SerializeField]
		private Button _resolveButton;

		[SerializeField]
		private Button _closeButton;

		private BoardingManager _manager;

		public void Begin() {
			_manager = BoardingManager.instance;

			float odds = _manager.GetInspectionOdds();
			SetFriendBarScale(odds);
			SetFoeBarScale(1 - odds);

			if (_manager.IsLegal()) {
				_oddsLabel.text = "We're completely legal";
			} else {
				if (odds == 1) {
					_oddsLabel.text = "They've got no chance";
				} else if (odds >= 0.75) {
					_oddsLabel.text = "Can't get much better than this";
				} else if (odds >= 0.5) {
					_oddsLabel.text = "We would be good... right?";
				} else if (odds >= 0.25) {
					_oddsLabel.text = "You don't need to look there...";
				} else {
					_oddsLabel.text = "Fuck";
				}
			}

			_resolveButton.gameObject.SetActive(true);
			_closeButton.gameObject.SetActive(false);

			Show();
		}

		private void SetFriendBarScale(float scale) {
			float totalWidth = _oddsBar.rect.width;
			
			Vector2 leftSize = _friendBar.offsetMax;
			leftSize.x = totalWidth * scale;

			_friendBar.offsetMax = leftSize;
		}

		private void SetFoeBarScale(float scale) {
			float totalWidth = _oddsBar.rect.width;

			Vector2 rightSize = _foeBar.offsetMin;
			rightSize.x = totalWidth * -scale;

			_foeBar.offsetMin = rightSize;
		}

		public void Resolve() {
			if (_manager.ResolveBoarding()) {
				_oddsLabel.text = "You are free to go, sir";

				_resolveButton.gameObject.SetActive(false);
				_closeButton.gameObject.SetActive(true);
			}
		}
	}
}