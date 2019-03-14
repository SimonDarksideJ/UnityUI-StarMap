using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LabelPool : MonoBehaviour {
	public GameObject objectPrefab;
	public Canvas canvas;
	public string poolParentName;

	private Transform canvasTransform;
	private List <Text> pool;
	private Transform poolParent;
	
	#region Reference Support
	private static LabelPool labelPool;
	
	public static LabelPool Instance () {
		if (!labelPool) {
			labelPool = FindObjectOfType(typeof (LabelPool)) as LabelPool;
			if (!labelPool) {
				Debug.LogError ("There needs to be one active LabelPool script on a GameObject in your scene.");
			}
		}
		
		return labelPool;
	}
	#endregion

	void Awake () {
		pool = new List<Text>();
	}

	void Start () {
		GameObject parentObject = new GameObject (poolParentName);
		poolParent = parentObject.transform;
		canvasTransform = canvas.GetComponent <Transform> ();
		poolParent.transform.SetParent (canvasTransform);
		poolParent.SetAsFirstSibling ();
	}

	public Text GetLabel() {
		foreach (Text label in pool) {
			if (label.gameObject.activeSelf == false) {
				return label;
			}
		}

		GameObject newLabelObject = Instantiate (objectPrefab, poolParent.position, Quaternion.identity) as GameObject;
		Text newLabel = newLabelObject.GetComponent <Text> ();
		pool.Add (newLabel);
		newLabel.transform.SetParent (poolParent);
	
		return newLabel;
	}
}