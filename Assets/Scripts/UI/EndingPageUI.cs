using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class EndingPageUI : Page {

		private static Settlement _respawnSettlement;
		
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
			_respawnSettlement = GameState.CanContinueGame();

			if (_respawnSettlement == null) {
				UIMain.OpenGameOverStory();
			} else {
				UIMain.OpenContinueStory();
			}
		}

		private void ContinueNext() {
			if (_respawnSettlement) {
				GameState.ContinueGameAtSettlement(_respawnSettlement);
				_respawnSettlement = null;
			}
		}

		private void GameOverNext() {
			GameState.RestartGame();
		}
	}
}