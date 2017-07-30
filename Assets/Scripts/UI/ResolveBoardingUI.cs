using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class ResolveBoardingUI : BoardingUI {

		[SerializeField]
		private RectTransform _oddsSlider;

		private float _sliderStopPoint = 0;
		private float _sliderDirection = 1;

		private bool _isSliderSliding = true;


		public override void Begin() {
			base.Begin();

			_oddsLabel.text = "";
			_oddsSlider.gameObject.SetActive(true);
			_resolveButton.gameObject.SetActive(false);

			float odds = _manager.GetInspectionOdds();
			if (_manager.ResolveBoarding()) {
				_sliderStopPoint = Random.Range(0, odds) * barWidth;
			} else {
				_sliderStopPoint = Random.Range(odds, 1) * barWidth;
			}

			_sliderStopPoint -= barWidth / 2;
		}


		protected override void SetupSpecialActions() {
			
		}

		void Update() {
			if (!_isSliderSliding) {
				return;
			}

			Vector2 pos = _oddsSlider.localPosition;
			pos.x += 300 * Time.deltaTime * _sliderDirection;

			if (Mathf.Abs(_sliderStopPoint - pos.x) < 5) {
				pos.x = _sliderStopPoint;
				CompleteBoarding();
			}

			if (pos.x < barWidth / -2) {
				pos.x = barWidth / -2;
				_sliderDirection *= -1;
			} else if (pos.x > barWidth / 2) {
				pos.x = barWidth / 2;
				_sliderDirection *= -1;
			}

			_oddsSlider.localPosition = pos;
		}

		private void CompleteBoarding() {
			_isSliderSliding = false;

			if (_manager.hasPassedBoarding) {
				_oddsLabel.text = "You're free to go, Captain";
				_resolveButton.gameObject.SetActive(true);
			} else {
				_oddsLabel.text = "We have a problem, sir";
				_closeButton.gameObject.SetActive(true);
			}
		}

		public override void Resolve() {
			Close();
		}

		public void PayFine() {
			UIMain.OpenPayFine(_manager);
		}
	}
}