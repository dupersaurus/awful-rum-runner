using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public abstract class Page : MonoBehaviour {

		private UIMain _ui;

		public UIMain ui {
			get { return _ui; }
			set { _ui = value; }
		}

		// Use this for initialization
		protected virtual void Start () {
			//Hide();
		}

		protected void Show() {
			gameObject.SetActive(true);
		}

		public virtual void Hide() {
			gameObject.SetActive(false);
		}

		public virtual void Close() {
			_ui.Close();
		}
	}
}