using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class BankUI : Page {

		public Text _settlementName;

		public Text _balanceLabel;
		public Text _balanceInterestLabel;
		public InputField _withdrawInput;

		public Text _cashLabel;
		public InputField _depositInput;

		public RectTransform _newLoanPanel;
		public Text _loanAvailableLabel;
		public Text _newLoanInterestLabel;
		public InputField _newLoanAmountInput;

		public InputField _payLoanInput;
		public RectTransform[] _loans;
		public Text[] _loanLabels;

		private SettlementService.Bank _bank;
		private PlayerAssets _assets;

		public void Show(SettlementService.Bank bank, PlayerAssets assets) {
			_bank = bank;
			_assets = assets;

			UpdateDisplay();
		}

		public void Withdraw() {
			int amt = int.Parse(_withdrawInput.text);
			_bank.Withdraw(amt);

			UpdateDisplay();

			_withdrawInput.text = "";
		}	

		public void Deposit() {
			int amt = int.Parse(_depositInput.text);
			_bank.Deposit(amt);

			UpdateDisplay();

			_depositInput.text = "";
		}

		public void PayLoan(int id) {
			var loans = _bank.GetLoans();

			if (id < 0 || id >= loans.Count) {
				return;
			}

			_bank.PayLoan(loans[id], int.Parse(_payLoanInput.text));
			_payLoanInput.text = "";

			UpdateDisplay();
		}

		public void TakeLoan() {
			_bank.TakeLoan(int.Parse(_newLoanAmountInput.text));
			_newLoanAmountInput.text = "";

			UpdateDisplay();
		}

		private void UpdateDisplay() {
			_settlementName.text = _bank.GetComponent<Settlement>().name;

			_balanceLabel.text = _bank.GetDeposit().ToString();
			_balanceInterestLabel.text = "@ " + (_bank.depositInterestRate * 100) + "%";

			_cashLabel.text = _assets.cash.ToString();

			_loanAvailableLabel.text = "up to " + _bank.loanAmount + " G";
			_newLoanInterestLabel.text = "@ " + (_bank.loanInterestRate * 100) + "%";

			var loans = _bank.GetLoans();

			_newLoanPanel.gameObject.SetActive(loans.Count < 3);
			_payLoanInput.gameObject.SetActive(loans.Count > 0);
			_loans[0].gameObject.SetActive(loans.Count == 1);
			_loans[1].gameObject.SetActive(loans.Count == 2);
			_loans[2].gameObject.SetActive(loans.Count == 3);

			for (int i = 0; i < loans.Count; i++) {
				_loanLabels[i].text = loans[i].amount + "G @ " + (loans[i].rate * 100) + "%";
			}
		}
	}
}