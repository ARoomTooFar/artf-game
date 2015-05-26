using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {
	// parsed char data storage
	public string char_name;
	public int char_id;
	public int hair_id;
	public int voice_id;
	public int money;
    public int[] inventory;
    public List<Weapons> weaponry;
    public List<Armor> armory;
    public List<Armor> hats;
    public List<Item> abilities;
    public List<GameObject> ammo;

	// data used to load player for gameplay
	public int weapon;
    public int headgear;
    public int armor;
	public int actionslot1;
	public int actionslot2;
	public int actionslot3;

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
