using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldItem : MonoBehaviour {

	public Text nameLabel;
	public Text countLabel;
	public GameObject illegalIcon;
	public Button dumpButton;

	public void SetCargo(Cargoes type, int amount) {
		var info = CargoManager.GetCargo(type);

		nameLabel.text = info.name;
		countLabel.text = amount.ToString();
		illegalIcon.SetActive(!info.legal);
	}
}
