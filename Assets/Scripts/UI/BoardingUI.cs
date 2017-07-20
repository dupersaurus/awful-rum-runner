using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class BoardingUI : Page {

		[SerializeField]
		protected RectTransform _oddsBar;

		[SerializeField]
		protected RectTransform _friendBar;
		
		[SerializeField]
		protected RectTransform _foeBar;

		[SerializeField]
		protected Text _oddsLabel;

		[SerializeField]
		protected Button _resolveButton;

		[SerializeField]
		protected Button _closeButton;

		protected BoardingManager _manager;

		public virtual void Begin() {
			_manager = BoardingManager.instance;

			float odds = _manager.GetInspectionOdds();
			SetFriendBarScale(odds);
			SetFoeBarScale(1 - odds);

			SetOddsLabel(odds);

			_resolveButton.gameObject.SetActive(true);
			_closeButton.gameObject.SetActive(false);

			Show();
		}

		protected virtual void SetOddsLabel(float odds) {
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
		}

		private void SetFriendBarScale(float scale) {
			float totalWidth = barWidth;
			
			Vector2 leftSize = _friendBar.offsetMax;
			leftSize.x = totalWidth * scale;

			_friendBar.offsetMax = leftSize;
		}

		private void SetFoeBarScale(float scale) {
			float totalWidth = barWidth;

			Vector2 rightSize = _foeBar.offsetMin;
			rightSize.x = totalWidth * -scale;

			_foeBar.offsetMin = rightSize;
		}

		protected float barWidth {
			get { return _oddsBar.rect.width; }
		}

		public virtual void Resolve() {
			float odds = _manager.GetInspectionOdds();

			if (odds == 1) {
				_oddsLabel.text = "You are free to go, sir";

				_resolveButton.gameObject.SetActive(false);
				_closeButton.gameObject.SetActive(true);
			} else {
				UIMain.OpenResolveBoarding();
			}
		}
	}
}