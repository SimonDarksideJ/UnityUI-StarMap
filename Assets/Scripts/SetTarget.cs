using UnityEngine;
using System.Collections;

public class SetTarget : MonoBehaviour {

	private CameraLookAt cameraLookAt;
	private HUDController hudController;
	private Star star;

	void Awake () {
		star = GetComponent <Star>();
	}

	void Start () {
		cameraLookAt = CameraLookAt.Instance ();
		hudController = HUDController.Instance ();
	}

	void OnMouseDown () {
		cameraLookAt.SetTarget (star.coordinates);
		hudController.SetStar (star);
	}
}
