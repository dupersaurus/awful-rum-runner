using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class TimeToBoardFloater : WorldSpaceFloater {
		
		[SerializeField]
		private Slider _slider;
		
		private float _progress = 0;

		public float progress {
			set { 
				_progress = value; 
				_slider.value = value;
			}
		}
	}
}