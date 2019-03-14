using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	private Transform thisTransform;
	private Transform mainCameraTransform;

	void Start () {
		thisTransform = this.transform;
		mainCameraTransform = Camera.main.transform;
	}

	void FixedUpdate () {
		thisTransform.LookAt(mainCameraTransform.position);
	}
}
