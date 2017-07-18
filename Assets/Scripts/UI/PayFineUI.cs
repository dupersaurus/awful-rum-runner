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

		public void Begin(BoardingManager manager) {
			_boarding = manager;
		}

		protected void UpdateDisplay() {
			_fineScaleText.text = _boarding.fineModifier.ToString();
			_cargoText.text = _boarding.GetIllegalCargoSize().ToString();
			_valueText.text = _boarding.GetIllegalCargoValue().ToString();
			_fineText.text = _boarding.GetFineCost().ToString();
			_playerCashText.text = GameState.assets.cash.ToString();
		}
	}
}
