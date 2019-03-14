using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Star : MonoBehaviour {
	public string starName;
	public Vector3 coordinates;
	public StarSystemDetails starSystemDetails;

	public LineRenderer line;
	public Collider col;

	[HideInInspector]
	public bool isInView = false;
	[HideInInspector]
	public Text label;
	[HideInInspector]
	public RectTransform labelRectTransform;
}