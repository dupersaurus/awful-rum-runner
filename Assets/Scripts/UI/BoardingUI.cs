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

		[SerializeField]
		private Button _escapeButton;

		[SerializeField]
		private Button _bribeRepButton;

		[SerializeField]
		private Button _bribeMoneyButton;

		[SerializeField]
		protected Text _goldLabel;

		[SerializeField]
		protected Text _reputationLabel;

		protected BoardingManager _manager;

		public virtual void Begin() {
			_manager = BoardingManager.instance;

			float odds = _manager.GetInspectionOdds();
			SetFriendBarScale(odds);
			SetFoeBarScale(1 - odds);

			SetOddsLabel(odds);

			_resolveButton.gameObject.SetActive(true);
			_closeButton.gameObject.SetActive(false);

			SetupSpecialActions();

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

		protected virtual void SetupSpecialActions() {
			int reputation = GameState.assets.reputation;
			int cash = GameState.assets.cash;
			int bribe = BoardingManager.GetBribeCost();

			_goldLabel.text = "G " + cash.ToString();
			_reputationLabel.text = "R " + reputation.ToString();

			_escapeButton.interactable = reputation >= 10000;
			_bribeRepButton.interactable = reputation >= bribe;
			_bribeMoneyButton.interactable = cash >= bribe;

			_escapeButton.onClick.AddListener(Escape);
			_bribeRepButton.onClick.AddListener(BribeReputation);
			_bribeMoneyButton.onClick.AddListener(BribeCash);

			Text label = FindChildNamed<Text>(_bribeRepButton.gameObject, "Cost");	

			if (label) {
				label.text = bribe + " R";
			}
			
			label = FindChildNamed<Text>(_bribeMoneyButton.gameObject, "Cost");	

			if (label) {
				label.text = bribe + " G";
			}
		}

		protected T FindChildNamed<T>(GameObject go, string name) where T: MonoBehaviour {
			var children = go.GetComponentsInChildren<T>();

			for (int i = 0; i < children.Length; i++) {
				if (children[i].gameObject.name == name) {
					return children[i];
				}
			}

			return null;
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

		private void Escape() {
			_manager.EscapeBoarding();
		}

		private void BribeCash() {
			_manager.BribeCash();
		}

		private void BribeReputation() {
			_manager.BribeReputation();
		}
	}
}