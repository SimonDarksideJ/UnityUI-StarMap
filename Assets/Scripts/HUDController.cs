using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUDController : MonoBehaviour {

	public Text starName;
	public Text starType;
	public Text allegiance;
	public Text economy;
	public Text government;
	public Text blackMarket;
	public Text station;
	
	public float defaultJumpDistance;
	public Text jumpDistanceLabel;
	public Slider jumpDistanceSlider;
	
	private Star currentStar;
	private float jumpDistance;
	private List <Star> starsInRange;
	
	#region Reference Support
	private static HUDController display;
	
	public static HUDController Instance () {
		if (!display) {
			display = FindObjectOfType(typeof (HUDController)) as HUDController;
			if (!display) {
				Debug.LogError ("There needs to be one active HUDController script on a GameObject in your scene.");
			}
		}
		
		return display;
	}
	#endregion

	void Awake () {
		jumpDistance = defaultJumpDistance;
		jumpDistanceSlider.value = jumpDistance;
		SetJumpDistance (jumpDistance);
		starsInRange = new List <Star>();
	}

	public void SetStar (Star newStar) {
		currentStar = newStar;
		SetStarData ();
		ClearJumpLines ();
		GatherStars (currentStar.coordinates, jumpDistance, starsInRange);
		CalculateJumpLines ();
	}

	void SetStarData () {
		starName.text = currentStar.starName;
//		starType.text = "Star Type";
//		allegiance.text = "Allegiance";
//		economy.text = "Economy";
//		government.text = "Government";
//		blackMarket.text = "Black Market";
		
//		starType.text = currentStar.starSystemDetails.starType;
//		allegiance.text = currentStar.starSystemDetails.allegiance;
//		economy.text = currentStar.starSystemDetails.economy;
//		government.text = currentStar.starSystemDetails.government;
//		blackMarket.text = currentStar.starSystemDetails.blackMarket;

		station.text = SetStations ();
	}

	string SetStations () {
		return "Station 01";
	}

	public void ClearJumpLines () {
		foreach (Star star in starsInRange) {
			star.line.SetPosition (1, star.coordinates);
			star.line.SetPosition (0, star.coordinates);
		} 
	}

	void GatherStars (Vector3 centerPoint, float distance, List <Star> starsInRange) {
		Collider[] hitColliders = Physics.OverlapSphere (centerPoint, distance);
		starsInRange.Clear();
		foreach (Collider hitCollider in hitColliders) {
			Star star = hitCollider.GetComponent <Star> ();
			starsInRange.Add (star);
		}
	}
	
	public void CalculateJumpLines () {
		foreach (Star star in starsInRange) {
			if (Vector3.Distance (star.coordinates, currentStar.coordinates) < jumpDistance) {
				star.line.SetPosition (0, star.coordinates);
				star.line.SetPosition (1, currentStar.coordinates);
			}
		}
	}
	
	public void SetJumpDistance (float newValue) {
		jumpDistance = newValue;
		if (currentStar) {
			ClearJumpLines ();
			GatherStars (currentStar.coordinates, jumpDistance, starsInRange);
			CalculateJumpLines ();
		}
	}
}