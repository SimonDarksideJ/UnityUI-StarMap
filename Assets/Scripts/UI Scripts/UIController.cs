using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	public void TogglePanel (GameObject panel) {
		panel.SetActive (!panel.activeSelf);
	}
}