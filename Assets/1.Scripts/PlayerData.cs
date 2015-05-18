using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	public string char_name;
	public int char_id;
	public int hair_id;
	public int voice_id;
	public int money;
	public int[] inventory = new int[10]; //size needs to be changed to final inventory size
}
