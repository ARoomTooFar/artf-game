using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	public string char_name;
	public int char_id;
	public int hair_id;
	public int voice_id;
	public int money;
	public int[] inventory = new int[52];

	public void PrintData() {
		Debug.Log ("// -- START PRINT PLAYER DATA -- //");
		Debug.Log (char_name);
		Debug.Log (char_id);
		Debug.Log (hair_id);
		Debug.Log (voice_id);
		Debug.Log (money);
		Debug.Log (inventory);
		foreach (int data in inventory) {
			Debug.Log (data);
		}
		Debug.Log ("// -- END PRINT PLAYER DATA -- //");
	}
}
