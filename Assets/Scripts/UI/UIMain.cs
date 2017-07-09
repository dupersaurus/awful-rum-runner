using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class UIMain : MonoBehaviour {

		private Page _activePage = null;

		public bool hasFocus {
			get { return _activePage != null; }
		}

		private WarehouseUI _warehouse;

		// Use this for initialization
		void Awake () {
			_warehouse = GetComponentInChildren<WarehouseUI>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Close() {
			if (_activePage) {
				_activePage.Hide();
			}

			_activePage = null;
		}
	}
}
