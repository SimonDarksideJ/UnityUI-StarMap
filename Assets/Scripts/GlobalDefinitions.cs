using UnityEngine;
using System.Collections;

public enum StarType {Red, Yellow, Blue};
public enum Allegiance {None, Independent, Federation};
public enum Economy {None, Agriculture, Extraction, Refinery, HighTech, Industrial};
public enum Government {};
public enum StationType {};

[System.Serializable]
public class StarSystemDetails {
	public StarType starType;
	public Allegiance allegiance;
	public Economy economy;
	public Government government;
	public int population;
	public bool blackMarket;
	public StationDetails[] Stations;
}

[System.Serializable]
public class StationDetails {
	public string stationName;
	public StationType stationType;
	public float distanceFromBeacon;
}
	
//[System.Serializable]
//public class StarLabel {
//	public Text labelText;
//	public bool isInView;
//	public Vector3 starCoordinates;
//}