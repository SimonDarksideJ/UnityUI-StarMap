using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TempStarLabel : MonoBehaviour {

	public Text label;

	// Use this for initialization
	void Start () {
		label.text = gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
