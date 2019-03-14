using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraLookAt : MonoBehaviour {
	
	public float distance;
	public Vector2 clampDistance;
	
	public float pivotSpeed;
	public float panSpeed;
	public float dollySpeed;
	public float smoothTime;

	private float x;
	private float y;
	private float xSmooth;
	private float ySmooth;
	private float xVelocity;
	private float yVelocity;
	private Vector3 posSmooth;
	private Vector3 posVelocity;
	private Transform mainCameraTransform;

	private Transform target;
	private Vector3 savedTargetCoordinates;

	private bool isOverUI;

	#region Reference Support
	private static CameraLookAt cameraLookAt;
	
	public static CameraLookAt Instance () {
		if (!cameraLookAt) {
			cameraLookAt = FindObjectOfType(typeof (CameraLookAt)) as CameraLookAt;
			if (!cameraLookAt) {
				Debug.LogError ("There needs to be one active CameraLookAt script on a GameObject in your scene.");
			}
		}
		
		return cameraLookAt;
	}
	#endregion

	void Awake () {
		posSmooth = transform.position;
		posVelocity = Vector3.zero;
		Vector3 angles = transform.eulerAngles;
		x = xSmooth = angles.y;
		y = ySmooth = angles.x;
		Camera mainCamera = Camera.main;
		mainCameraTransform = mainCamera.transform;
		GameObject targetObject = new GameObject ("Camera Target");
		target = targetObject.transform;
		PlayerPrefs.DeleteAll();
		savedTargetCoordinates = GetSavedCoordinates ();
		SetTarget (savedTargetCoordinates);
		isOverUI = false;
	}

	void LateUpdate () {

		distance -= Input.GetAxisRaw ("Mouse ScrollWheel");
		distance = Mathf.Clamp (distance, clampDistance.x, clampDistance.y);
		
		if (!isOverUI && Input.GetMouseButton (0) && !Input.GetMouseButton (1)) {
			x += Input.GetAxis ("Mouse X") * pivotSpeed;
			y -= Input.GetAxis ("Mouse Y") * pivotSpeed;
		}

		if (!isOverUI && !Input.GetMouseButton (0) && Input.GetMouseButton (1)) {
			Vector3 up = mainCameraTransform.TransformDirection(Vector3.up);
			Vector3 right = mainCameraTransform.TransformDirection(Vector3.left);
			float mouseX = Input.GetAxis ("Mouse X") * dollySpeed;
			float mouseY = Input.GetAxis ("Mouse Y") * dollySpeed;
			target.position += ((right * mouseX) + (up * -mouseY));
		}
		xSmooth = Mathf.SmoothDamp (xSmooth, x, ref xVelocity, smoothTime);
		ySmooth = Mathf.SmoothDamp (ySmooth, y, ref yVelocity, smoothTime);

		Quaternion rotation = Quaternion.Euler (ySmooth, xSmooth, 0.0f);

		posSmooth = Vector3.SmoothDamp(posSmooth, target.position,ref posVelocity, smoothTime);

		transform.rotation = rotation;
		transform.position = rotation * new Vector3(0.0f, 0.0f, -distance) + posSmooth;
	}

	public void SetTarget (Vector3 targetCoordinates) {
		target.position = targetCoordinates;
		SetSavedCoordinates (targetCoordinates);
	}

	Vector3 GetSavedCoordinates () {
		return new Vector3 (PlayerPrefs.GetFloat ("xCoord", -19.75f), PlayerPrefs.GetFloat ("yCoord", 41.78125f), PlayerPrefs.GetFloat ("zCoord", -3.1875f));
//		return new Vector3 (PlayerPrefs.GetFloat ("xCoord", 9965.25f), PlayerPrefs.GetFloat ("yCoord", 1026.78f), PlayerPrefs.GetFloat ("zCoord", 4101.81f));
	}

	void SetSavedCoordinates (Vector3 coordinates) {
		PlayerPrefs.SetFloat ("xCoord", coordinates.x);
		PlayerPrefs.SetFloat ("yCoord", coordinates.y);
		PlayerPrefs.SetFloat ("zCoord", coordinates.z);
	}

	public void SetOverUI (bool toggle) {
		isOverUI = toggle;
	}
}
