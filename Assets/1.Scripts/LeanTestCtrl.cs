using UnityEngine;
using System.Collections;

public class LeanTestCtrl : MonoBehaviour {
	private Farts serv;
	private string charDataEx = "80PercentLean,123,456,789,9001,0,1,2,3";

	// Use this for initialization
	void Start () {
		PlayerData playerData = gameObject.AddComponent<PlayerData> ();
		serv = gameObject.AddComponent<Farts> ();

		playerData = serv.parseCharData(charDataEx);

		string stringifiedCharData = serv.stringifyCharData (playerData);
		Debug.Log (stringifiedCharData);
	}
}
