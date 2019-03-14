using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(InputField))]
public class SetInputFieldValue : MonoBehaviour {

	private InputField inputField;

	void Awake () {
		inputField = GetComponent <InputField> ();
	}

	public void SetValue (float value)
	{
        if (inputField != null)
        {
            inputField.text = (value.ToString("n2"));
        }
    }
}