using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class PayFineUI : Page {

		private BoardingManager _boarding;

		[SerializeField]
		private Text _cargoText;

		[SerializeField]
		private Text _valueText;

		[SerializeField]
		private Text _fineScaleText;

		[SerializeField]
		private Text _fineText;

		[SerializeField]
		private Text _playerCashText;

		[SerializeField]
		private GameObject _payButton;

		[SerializeField]
		private GameObject _jailButton;

		[SerializeField]
		private Button _reduce3xButton;

		[SerializeField]
		private Button _reduce2xButton;

		[SerializeField]
		private Button _reduce1xButton;

		[SerializeField]
		private Text _reputationLabel;

		private bool _canPayFine = false;

		public void Begin(BoardingManager manager) {
			_boarding = manager;

			UpdateDisplay();
		}

		protected void UpdateDisplay() {
			int fine = _boarding.GetFineCost();
			int cash = GameState.assets.cash;

			_fineScaleText.text = _boarding.fineModifier.ToString();
			_cargoText.text = _boarding.GetIllegalCargoSize().ToString();
			_valueText.text = _boarding.GetIllegalCargoValue().ToString();
			_fineText.text = fine.ToString();
			_playerCashText.text = cash.ToString();

			if (cash >= fine) {
				_canPayFine = true;
				_fineText.color = Color.cyan;
				_payButton.SetActive(true);
				_jailButton.SetActive(false);
			} else {
				_canPayFine = false;
				_fineText.color = new Color(1, 0.5f, 0, 1);
				_payButton.SetActive(false);
				_jailButton.SetActive(true);
			}

			// Reputation
			int reputation = GameState.assets.reputation;
			_reputationLabel.text = "R " + reputation;

			_reduce3xButton.interactable = reputation >= 15000 && _boarding.fineModifier > 3;
			_reduce2xButton.interactable = reputation >= 10000 && _boarding.fineModifier > 2;
			_reduce1xButton.interactable = reputation >= 5000 && _boarding.fineModifier > 1;
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

		public void ReduceFine(int amt) {
			_boarding.ReduceFine(amt);
			UpdateDisplay();
		}

		public void Resolve() {
			
		}
	}
}
