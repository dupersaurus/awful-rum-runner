using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class EndingPageUI : Page {
		
		public void Continue(string page) {
			switch (page) {
				case "arrested":
					ArrestedNext();
					break;

				case "continue":
					ContinueNext();
					break;

				case "game over":
					GameOverNext();
					break;
			}
		}

		private void ArrestedNext() {
			var settlement = GameState.CanContinueGame();

			if (settlement == null) {
				UIMain.OpenGameOverStory();
			} else {
				UIMain.OpenContinueStory();
			}
		}

		private void ContinueNext() {

		}

		private void GameOverNext() {

		}
	}
}