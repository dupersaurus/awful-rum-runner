﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager {

	private const int DAY = 86400;

	private const int HOUR = 3600;

	private int _currentDay = 1;

	private float _currentTime = 0;

	/// <summary>
	/// How many in-game seconds for one real-life second
	/// </summary>
	private int _timeScale = 1200;

	/// <summary>
	/// The current day
	/// </summary>
	/// <returns></returns>
	public int day {
		get { return _currentDay; }
	}

	public int hour {
		get { return Mathf.FloorToInt(_currentTime / HOUR); }
	}

	public int minute {
		get { return Mathf.FloorToInt((_currentTime % HOUR) / 60); }
	}

	public TimeManager() {

	}

	// Update is called once per frame
	public void Update (float delta) {
		_currentTime += delta * _timeScale;

		if (_currentTime >= DAY) {
			_currentDay++;
			_currentTime -= DAY;
		}
	}

	public void AddDays(int days) {
		_currentDay += days;
	}

	public void AddHours(int hours) {
		_currentTime += hours * HOUR;
	}

	public void SetTime(int day, int hour) {
		_currentDay = day;
		_currentTime = hour * HOUR;
	}

	public void StartDay(int day) {
		_currentDay = day;
		_currentTime = 12 * HOUR;
	}
}
