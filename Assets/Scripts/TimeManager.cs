using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager {

	private int _currentDay = 1;

	private float _currentTime = 0;

	private int _timeScale = 5;

	public TimeManager() {

	}

	// Update is called once per frame
	public void Update (float delta) {
		_currentTime += delta * _timeScale;

		if (_currentTime >= 86400) {
			_currentDay++;
			_currentTime -= 86400;
		}
	}
}
