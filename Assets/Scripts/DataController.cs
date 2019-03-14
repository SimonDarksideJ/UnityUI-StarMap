using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

#region class definitions
[System.Serializable]
public class StarQuery {
    public string systemName = string.Empty;
    public int showId = 0;
	public int showCoordinates = 1;
	public int showPermit = 0;
    public int showInformation = 0;
    public int showPrimaryStar = 0;
    /// <summary>
    /// Format as YYYY-MM-DD HH:MM:SS
    /// </summary>
    public string startDateTime = string.Empty;
    /// <summary>
    /// Format as YYYY-MM-DD HH:MM:SS
    /// </summary>
    public string endDateTime = string.Empty;
    public int onlyFeatured = 0;
    public int onlyKnownCoordinates = 1;
    public int onlyUnknownCoordinates = 0;
    public int includeHidden = 0;
}

[System.Serializable]
public class RawData {
	public SystemData[] systems;
}

[System.Serializable]
public class SystemData {
	public string name;
	public int id;
    public string duplicates;
    public Coordinates coords;
    public string requirePermit;
    public string permitName;
    public string information;
    public string primaryStar;
    public string hidden_at;
    public string mergedTo;
    public bool coordsLocked;
}

[System.Serializable]
public class Coordinates {
	public double x;
	public double y;
	public double z;
}

[System.Serializable]
public class URLs {
	public string getSystems;
	public string getDistances;
	public string setDistances;
}
#endregion

public class DataController : MonoBehaviour
{
    private URLs urls;
    public StarQuery query;
    private string serializedQuery;
    private RawData rawData;
    private MapController mapController;

    void Awake()
    {
        urls = new URLs();
        urls.getSystems = "https://www.edsm.net/api-v1/systems";
        urls.getDistances = "http://edstarcoordinator.com/api.asmx/GetDistances";
        urls.setDistances = "http://edstarcoordinator.com/api.asmx/SetDistances";
        rawData = new RawData();
    }

    void Start()
    {
        mapController = GetComponent<MapController>();
    }

    public RawData GetStarData()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/StarData.json"))
        {
            string data = System.IO.File.ReadAllText(Application.persistentDataPath + "/StarData.json");
            ProcessStarData(data);
            return rawData;
        }
        else
        {
            Debug.LogWarning("StarData.json is Missing! GetStarData returns no data! Update StarData and try again!");
            return null;
        }
    }

    public void RefreshStarData()
    {
        Debug.Log("Updating!");
        StartCoroutine(UpdateStarData(query));
    }

    IEnumerator UpdateStarData(StarQuery query)
    {
        //query.systemName = "Thailoae FN-L b40-1";
        //query.showCoordinates = 1;

        UnityWebRequest unityWebRequest = UnityWebRequest.Get(urls.getSystems + GetRequestQuery(query));

        unityWebRequest.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.error != null)
        {
            Debug.LogWarning("Error updating star data! " + unityWebRequest.error);
            // Popup warning dialogue
        }
        else
        {
            ProcessStarData(unityWebRequest.downloadHandler.text);
            mapController.CreateMap();
        }
    }

    void ProcessStarData(string data)
    {
        rawData = JsonUtility.FromJson<RawData>("{\"systems\":" + data + "}");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/StarData.json", data);
    }

    string GetRequestQuery(StarQuery query)
    {
        string queryString = "?";

        if (query.systemName != string.Empty) queryString = queryString + "systemName=" + query.systemName + "&";
        if (query.showId == 1) queryString = queryString + "showId=" + query.showId + "&";
        if (query.showCoordinates == 1) queryString = queryString + "showCoordinates=" + query.showCoordinates + "&";
        if (query.showPermit == 1) queryString = queryString + "showPermit=" + query.showPermit + "&";
        if (query.showInformation == 1) queryString = queryString + "showInformation=" + query.showInformation + "&";
        if (query.showPrimaryStar == 1) queryString = queryString + "sysshowPrimaryStartemName=" + query.showPrimaryStar + "&";
        if (query.startDateTime != string.Empty) queryString = queryString + "startDateTime=" + query.startDateTime + "&";
        if (query.endDateTime != string.Empty) queryString = queryString + "endDateTime=" + query.endDateTime + "&";
        if (query.onlyFeatured == 1) queryString = queryString + "onlyFeatured=" + query.onlyFeatured + "&";
        if (query.onlyKnownCoordinates == 1) queryString = queryString + "onlyKnownCoordinates=" + query.onlyKnownCoordinates + "&";
        if (query.onlyUnknownCoordinates == 1) queryString = queryString + "onlyUnknownCoordinates=" + query.onlyUnknownCoordinates + "&";
        if (query.includeHidden == 1) queryString = queryString + "includeHidden=" + query.includeHidden + "&";

        return queryString.Substring(0,queryString.Length - 1);
    }
}