using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.ParticleSystem;

public class MapController : MonoBehaviour {

	public TextAsset mapData;
	public GameObject starPrefab;
	public GameObject starLabel;
	public Vector2 clampViewDistance;
	public Vector3 labelOffset;
	public float starSize;

	private DataController dataController;
	public RawData rawData;							//	Make Private After Testing

	private List <Star> starSystems;
	private List <Star> viewableStars;
	private ParticleSystem starParticles;
	private Camera mainCamera;
	private Transform mainCameraTransform;

	private LabelPool labelPool;

	void Awake () {
		viewableStars = new List <Star> ();
		starParticles = GetComponent <ParticleSystem>  ();
		dataController = GetComponent <DataController> ();
	}

	void Start () {
		mainCamera = Camera.main;
		mainCameraTransform = mainCamera.transform;
		labelPool = LabelPool.Instance ();
		CreateMap ();
	}

	public void CreateMap () {
		starSystems = new List <Star> ();
		GameObject starParent = new GameObject ("Star Parent");
		Transform starParentTransform = starParent.transform;

		rawData = dataController.GetStarData ();
		if (rawData == null) {
			Debug.LogWarning ("Raw Data is NULL from DataController.GetStarData. Please refresh star data and try again.");
			return;
		}

		List <SystemData> rawStarData = new List<SystemData>(rawData.systems);
		foreach (var starSystem in rawStarData) {

			GameObject starObject = Instantiate (starPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			starObject.transform.parent = starParentTransform;
			starObject.name = starSystem.name;

			Star star = starObject.GetComponent <Star> ();
			star.starName = starSystem.name;
            if (starSystem.coords != null)
            {
                star.coordinates = new Vector3((float)starSystem.coords.x, (float)starSystem.coords.y, (float)starSystem.coords.z);
            }
            starObject.transform.position = star.coordinates;
            var emitParams = new EmitParams()
            {
                position = star.coordinates,
                velocity = Vector3.zero,
                startSize = starSize,
                startLifetime = Mathf.Infinity,
                startColor = Color.white
            };
            starParticles.Emit (emitParams,1);
			starSystems.Add (star);
		}
	}

	void Update () {
		// Push this to a coroutine
		foreach (Star star in starSystems) {
			bool viewCheck = CheckStarIsInView (star.coordinates);
			if (viewCheck != star.isInView) {
				if (viewCheck == true) {
					star.label = labelPool.GetLabel();
					star.label.gameObject.SetActive (true);
					star.labelRectTransform = star.label.transform as RectTransform;
					viewableStars.Add (star);
					star.isInView = true;
					star.label.text = star.starName;
				} else {
					star.label.gameObject.SetActive (false);
					viewableStars.Remove (star);
					star.labelRectTransform = null;
					star.label = null;
					star.isInView = false;
				}
			} 
		}

		foreach (Star star in viewableStars) {
			star.label.transform.position = mainCamera.WorldToScreenPoint (star.coordinates) + labelOffset;;
			float labelAlpha = SetLabelAlpha (star.coordinates);
			Color labelColor = new Color (star.label.color.r, star.label.color.g, star.label.color.b, labelAlpha);
			star.label.color = labelColor;
		}
	}

	bool CheckStarIsInView (Vector3 coordinates) {
		Vector3 forward = mainCameraTransform.TransformDirection(Vector3.forward);
		Vector3 toOther = coordinates - mainCameraTransform.position;
		if (SetLabelAlpha (coordinates) > 0) {
			if (Vector3.Dot(forward, toOther) > 0) {
				Vector2 screenPosition = mainCamera.WorldToViewportPoint (coordinates);
				if (screenPosition.x > -0.1f && screenPosition.y > -0.1f && screenPosition.x < 1.1f && screenPosition.y < 1.1f) {
					return true;
				}
			}
		}
		return false;
	}
	
	float SetLabelAlpha (Vector3 coordinates) {
		float labelDistance = Vector3.Distance (mainCameraTransform.position, coordinates);
		float baseRange = clampViewDistance.y - clampViewDistance.x;
		float returnValueRaw = Mathf.Clamp ((labelDistance - baseRange)/baseRange, 0.0f, 1.0f);
		float returnValue = 1.0f - returnValueRaw;
		return returnValue;
	}
}
