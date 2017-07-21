using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class DateUI : MonoBehaviour {

		[SerializeField]
		private Text _dateText;
		
		// Update is called once per frame
		void Update () {
			TimeManager time = GameState.time;
			string hours = "";
			string minutes = "";

			if (time.hour >= 10) {
				hours = time.hour.ToString();
			} else {
				hours = "0" + time.hour;
			}

			if (time.minute >= 10) {
				minutes = time.minute.ToString();
			} else {
				minutes = "0" + time.minute;
			}

			_dateText.text = "Day " + time.day + ", " + hours + ":" + minutes;
		}
	}
}