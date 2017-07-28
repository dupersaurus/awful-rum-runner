using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager {

	public delegate void OnNewDay(int day);

	public event OnNewDay newDay;

	private const int DAY = 86400;

	private const int HOUR = 3600;

	private int _currentDay = 1;

	private float _currentTime = 0;

	/// <summary>
	/// How many in-game seconds for one real-life second
	/// </summary>
	private int _timeScale = 1200;

	private DayAndNightControl _dayNightController;

	/// <summary>
	/// The current day
	/// </summary>
	/// <returns></returns>
	public int day {
		get { return _dayNightController.currentDay; }
	}

	public int hour {
		get { return Mathf.FloorToInt(seconds / HOUR); }
	}

	public int minute {
		get { return Mathf.FloorToInt((seconds % HOUR) / 60); }
	}

	public float seconds {
		get { return _dayNightController.currentTime * 86400; }
	}

	public float lightLevel {
		get { return _dayNightController.lightLevel; }
	}

	public TimeManager() {
		_dayNightController = GameObject.FindObjectOfType<DayAndNightControl>();
	}

	// Update is called once per frame
	public void Update (float delta) {
		if (_dayNightController.CallUpdate(delta)) {
			// New day
			if (newDay != null) {
				newDay(_dayNightController.currentDay);
			}
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
