using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class FloatingIcons : MonoBehaviour {

		// Update is called once per frame
		void Update () {
			
		}

		public WorldSpaceFloater AddWorldFloater(string prefab, Transform target) {
			GameObject go = Instantiate(Resources.Load("UI/" + prefab)) as GameObject;
			WorldSpaceFloater f = go.GetComponent<WorldSpaceFloater>();

			if (f) {
				go.transform.SetParent(GetComponent<RectTransform>(), false);
				f.track = target;

				return f;
			} else {
				DestroyImmediate(go);
				return null;
			}
		}
	}
}