using UnityEngine;
using System.Collections;

public class LeanTestCtrl : MonoBehaviour {
	private Farts serv;
    private string charDataEx = "1337,123,456,789,9001,0,1,3,2,4,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";

	// Use this for initialization
	void Start () {
        PlayerData playerData = new PlayerData();
		serv = gameObject.AddComponent<Farts> ();

		playerData = serv.parseCharData(charDataEx);

		Debug.Log (playerData.game_acct_id);
		Debug.Log (playerData.char_id);
		Debug.Log (playerData.hair_id);
		Debug.Log (playerData.voice_id);
		Debug.Log (playerData.money);
		Debug.Log (playerData.inventory);
		foreach (int data in playerData.inventory) {
			Debug.Log (data);
		}

		string stringifiedCharData = serv.stringifyCharData (playerData);
		Debug.Log (stringifiedCharData);
	}
}
