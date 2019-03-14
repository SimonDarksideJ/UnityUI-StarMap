using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class SetSliderValue : MonoBehaviour {

	private Slider slider;

	void Awake () {
		slider = GetComponent<Slider>();
	}

	public void SetValue (string inputValue)
	{
		slider.value = float.Parse (inputValue);
	}
}
