using UnityEngine;
using System.Collections;

public class TESTSCRIPT : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MapData.addRoom(new Vector3(0, 0, 0), new Vector3(1, 0, 1));

		string save = MapData.SaveString;
		print(save);

		MapDataParser.ParseSaveString(save);

		string newSave = MapData.SaveString;
		print(newSave);
	}
}
