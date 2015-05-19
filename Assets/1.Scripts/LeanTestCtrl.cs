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

		Debug.Log (playerData.name);
		Debug.Log (playerData.char_id);
		Debug.Log (playerData.hair_id);
		Debug.Log (playerData.voice_id);
		Debug.Log (playerData.money);
		Debug.Log (playerData.inventory);
		foreach (int data in playerData.inventory) {
			Debug.Log (data);
		}
	}
}
