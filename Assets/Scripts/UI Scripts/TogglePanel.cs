using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TogglePanel : MonoBehaviour {

	public void Toggle ()
	{
		gameObject.SetActive (!gameObject.activeSelf);;
	}
}