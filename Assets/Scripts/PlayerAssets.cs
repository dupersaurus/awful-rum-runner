using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanStructure {
	private string _bank;
	private int _amount;
	private float _rate;

	public string bank {
		get { return _bank; }
	}

	public int amount {
		get { return _amount; }
	}

	public float rate {
		get { return _rate; }
	}

	public LoanStructure(string bank, int amt, float rate) {
		_bank = bank;
		_amount = amt;
		_rate = rate;
	}

	public void CalculateInterest() {
		_amount += Mathf.CeilToInt(_amount * _rate);
	}

	public void PayBalance(int amount) {
		_amount -= amount;
	}
}

/// <summary>
/// The player's personal fortune
/// </summary>
public class PlayerAssets {

	/// <summary>
	/// Cash on hand
	/// </summary>
	private int _cash = 200;

	/// <summary>
	/// Reputation is earned for selling illicit items
	/// </summary>
	private int _reputation = 0;

	/// <summary>
	/// Cash in each bank in the world
	/// </summary>
	private Dictionary<string, int> _bank = new Dictionary<string, int>();

	/// <summary>
	/// Debts owed to each bank in the world
	/// </summary>
	private List<LoanStructure> _debts = new List<LoanStructure>();

	public int cash {
		get { return _cash; }
	}

	public int reputation {
		get { return _reputation; }
	}

	public void Initialize(TimeManager time) {
		time.newDay += OnNewDay;
	}

	void OnNewDay(int day) {
		for (int i = 0; i < _debts.Count; i++) {
			_debts[i].CalculateInterest();
		}
	}

	/// <summary>
	/// Modifies the cash on hand by a certain amount. The cash on hand cannot
	/// go below 0.
	/// </summary>
	/// <param name="amount"></param>
	public void ModifyCash(int amount) {
		_cash += amount;

		if (_cash < 0) {
			_cash = 0;
		}
	}

	public void ModifyReputation(int amount) {
		_reputation += amount;

		if (_reputation < 0) {
			_reputation = 0;
		}
	}

	public int GetDeposit(string name) {
		if (_bank.ContainsKey(name)) {
			return _bank[name];
		} else {
			return 0;
		}
	}

	/// <summary>
	/// Sets the amount of money in a bank at a settlement
	/// </summary>
	/// <param name="name"></param>
	/// <param name="amount"></param>
	/// <returns></returns>
	protected void SetDeposit(string name, int amount) {
		if (amount < 0) {
			amount = 0;
		}

		if (_bank.ContainsKey(name)) {
			_bank[name] = amount;
		} else {
			_bank.Add(name, amount);
		}
	}

	/// <summary>
	/// Deposit into bank without taking from cash
	/// </summary>
	/// <param name="name"></param>
	/// <param name="amount"></param>
	public void CreditDeposit(string name, int amount) {
		SetDeposit(name, amount + GetDeposit(name));
	}

	public void Deposit(string name, int amount) {
		if (amount > _cash) {
			amount = _cash;
		}

		_cash -= amount;

		SetDeposit(name, amount + GetDeposit(name));
	}

	public void Withdraw(string name, int amount) {
		if (GetDeposit(name) < amount) {
			amount = GetDeposit(name);
		}

		_cash += amount;
		SetDeposit(name, GetDeposit(name) - amount);
	}

	public void AddLoan(string settlement, int amount, float rate) {
		_debts.Add(new LoanStructure(settlement, amount, rate));
		_cash += amount;
	}

	public int PayLoan(LoanStructure loan, int amount) {
		if (amount > loan.amount) {
			amount = loan.amount;
		}
		
		if (amount > _cash) {
			amount = _cash;
		}

		_cash -= amount;
		loan.PayBalance(amount);

		if (loan.amount <= 0) {
			_debts.Remove(loan);
		}

		return amount;
	}

	public List<LoanStructure> GetSettlementLoans(string settlement) {
		var loans = new List<LoanStructure>();

		for (int i = 0; i < _debts.Count; i++) {
			if (_debts[i].bank == settlement) {
				loans.Add(_debts[i]);
			}
		}

		return loans;
	}
}
