using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettlementService {
	public class Bank : MonoBehaviour, ISettlementService {

		/// <summary>
		/// The interest rate on deposits
		/// </summary>
		[SerializeField]
		private float _depositIntrestRate = 0.01f;

		public float depositInterestRate {
			get { return _depositIntrestRate; }
		}

		/// <summary>
		/// The base interest rate on loans
		/// </summary>
		[SerializeField]
		private float _loanInterestRate = 0.01f;

		public float loanInterestRate {
			get { return _loanInterestRate; }
		}

		/// <summary>
		/// How much the interest rate goes up with each loan taken
		/// </summary>
		[SerializeField]
		private float _loanInterestRateIncrement = 0.02f;

		/// <summary>
		/// The amount the settlement can loan
		/// </summary>
		[SerializeField]
		private int _loanAmount = 500;

		/// <summary>
		/// The amount the settlement can loan
		/// </summary>
		public int loanAmount {
			get { return _loanAmount; }
		}

		public void Initialize() {
			GameState.time.newDay += OnNewDay;
		}

		void OnNewDay(int day) {
			Deposit(Mathf.CeilToInt(GetDeposit() * _depositIntrestRate));
		}

		/// <summary>
		/// Amount of money deposited in the bank
		/// </summary>
		/// <returns></returns>
		public int GetDeposit() {
			return GameState.assets.GetDeposit(gameObject.name);
		}

		/// <summary>
		/// Loans taken with the bank
		/// </summary>
		/// <returns></returns>
		public List<LoanStructure> GetLoans() {
			return GameState.assets.GetSettlementLoans(gameObject.name);
		}

		/// <summary>
		/// Deposit money into the bank
		/// </summary>
		/// <param name="amount"></param>
		public void Deposit(int amount) {
			GameState.assets.Deposit(gameObject.name, amount);
		}

		/// <summary>
		/// Withdraw money from the bank
		/// </summary>
		/// <param name="amount"></param>
		public void Withdraw(int amount) {
			GameState.assets.Withdraw(gameObject.name, amount);
		}

		/// <summary>
		/// Pay an amount on a loan
		/// </summary>
		/// <param name="loan"></param>
		/// <param name="amount"></param>
		public void PayLoan(LoanStructure loan, int amount) {
			_loanAmount += GameState.assets.PayLoan(loan, amount);
		}

		/// <summary>
		/// Take out a loan at the current rate
		/// </summary>
		/// <param name="amount"></param>
		public void TakeLoan(int amount) {
			GameState.assets.AddLoan(gameObject.name, amount, CalculateLoanRate(amount));
			_loanAmount -= amount;
		}

		/// <summary>
		/// Calculate a rate for a given loan
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public float CalculateLoanRate(int amount) {
			return _loanInterestRate + GetLoans().Count * _loanInterestRateIncrement;
		}
	}
}