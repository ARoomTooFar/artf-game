using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointUpdater : MonoBehaviour {

	CameraRaycast camCast;
	Text txt;
	ARTFRoom rm;

	// Use this for initialization
	void Start () {
		camCast = Camera.main.GetComponent<CameraRaycast>();
		txt = GameObject.Find("Points").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		rm = MapData.TheFarRooms.find(camCast.mouseGroundPoint.Round());
		txt.text = rm == null ? "--/--" : rm.PointString;
	}
}
