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
	public bool[] activePlayers;
	public bool actable,loaded;
	public int numPlayers;
	//Like the data chest, just a one part thing that deals with all the icons being stored so they are accessible in that location
	//May merge with selector at some point but who knows~
	// Use this for initialization

    private GSManager gsManager;

	void Start () {
		actable = false;
        gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if((!inventories[0].on || inventories[0].ready) && (!inventories[0].on || inventories[0].ready) && (!inventories[0].on || inventories[0].ready) && (!inventories[0].on || inventories[0].ready)){
			if(!inventories[0].on && !inventories[1].on && !inventories[2].on && !inventories[3].on){
				actable = false;
			}else{
				StartCoroutine(Wait(.15f));
			}
		}
		if(actable){
			playerCount();
			if((Input.GetKeyDown(inventories[0].controls.attack) || (inventories[0].controls.joyUsed &&  Input.GetButtonDown(inventories[0].controls.joyAttack)))&&loaded) {
				actable = false;

                Debug.Log(gsManager.currLevelId);
				//Application.LoadLevel("ProtoPaul");
                gsManager.LoadLevel(gsManager.currLevelId);
			}else if((Input.GetKeyDown(inventories[1].controls.attack) || (inventories[1].controls.joyUsed &&  Input.GetButtonDown(inventories[1].controls.joyAttack)))&&loaded) {
				actable = false;
		
				Application.LoadLevel("TitleScreen2");
			}else if((Input.GetKeyDown(inventories[2].controls.attack) || (inventories[2].controls.joyUsed &&  Input.GetButtonDown(inventories[2].controls.joyAttack)))&&loaded) {
				actable = false;
			
				Application.LoadLevel("TitleScreen2");
			}else if((Input.GetKeyDown(inventories[3].controls.attack) || (inventories[3].controls.joyUsed &&  Input.GetButtonDown(inventories[3].controls.joyAttack)))&&loaded) {
				actable = false;
			
				Application.LoadLevel("TitleScreen2");
			}
		}
	}
	private void playerCount(){
		if(inventories[0].on && inventories[0].ready){
			numPlayers++;
			activePlayers[0] = true;
			inventories[0].recompileText("P" + (inventories[0].playNumber).ToString());
		}if(inventories[1].on && inventories[1].ready){
			numPlayers++;
			activePlayers[1] = true;
			inventories[1].recompileText("P" + (inventories[1].playNumber).ToString());
		}if(inventories[2].on && inventories[2].ready){
			numPlayers++;
			activePlayers[2] = true;
			inventories[2].recompileText("P" + (inventories[2].playNumber).ToString());
		}if(inventories[3].on && inventories[3].ready){
			numPlayers++;
			activePlayers[3] = true;
			inventories[3].recompileText("P" + (inventories[3].playNumber).ToString());
		}
		if(!loaded){
			loaded = true;
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
	private IEnumerator lWait(float duration){
		if(!loaded){
			for (float timer = 0; timer < duration; timer += Time.deltaTime){
				yield return 0;
			}
			loaded = true;
		}
	}
}
