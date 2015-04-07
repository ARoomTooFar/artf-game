using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IconChest : MonoBehaviour {
	public List<Material> armory;
	public List<Material> hats;
	public List<Material> inventory;
	public List<Material> weaponry;
	public Material offState;
	public Inventory[] inventories;
	public bool actable;
	public int numPlayers;
	//Like the data chest, just a one part thing that deals with all the icons being stored so they are accessible in that location
	//May merge with selector at some point but who knows~
	// Use this for initialization
	void Start () {
		actable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if((!inventories[0].on || inventories[0].ready) && (!inventories[0].on || inventories[0].ready) && (!inventories[0].on || inventories[0].ready) && (!inventories[0].on || inventories[0].ready)){
			if(!inventories[0].on && !inventories[1].on && !inventories[2].on && !inventories[3].on){
				actable = false;
			}else{
				StartCoroutine(Wait(.25f));
			}
		}
		if(actable){
			if(Input.GetKeyDown(inventories[0].controls.attack) || (inventories[0].controls.joyUsed &&  Input.GetButtonDown(inventories[0].controls.joyAttack))) {
				if(inventories[0].ready){
					numPlayers++;
				}
				Application.LoadLevel("TitleScreen2");
			}
			if(Input.GetKeyDown(inventories[1].controls.attack) || (inventories[1].controls.joyUsed &&  Input.GetButtonDown(inventories[1].controls.joyAttack))) {
				Application.LoadLevel("TitleScreen2");
			}
			if(Input.GetKeyDown(inventories[2].controls.attack) || (inventories[2].controls.joyUsed &&  Input.GetButtonDown(inventories[2].controls.joyAttack))) {
				Application.LoadLevel("TitleScreen2");
			}
			if(Input.GetKeyDown(inventories[3].controls.attack) || (inventories[3].controls.joyUsed &&  Input.GetButtonDown(inventories[3].controls.joyAttack))) {
				Application.LoadLevel("TitleScreen2");
			}
		}
	}
	private IEnumerator Wait(float duration){
		if(!actable){
			for (float timer = 0; timer < duration; timer += Time.deltaTime){
				yield return 0;
			}
			actable = true;
		}
	}
}
