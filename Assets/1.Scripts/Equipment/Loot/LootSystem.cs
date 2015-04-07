using UnityEngine;
using System.Collections;

public class LootSystem : MonoBehaviour {
	public int stage; //0-15
	public float rating;
	public int[] rarity;
	public CameraAdjuster cameraAdj;
	public bool inTest;
	// Use this for initialization
	void Start () {
		cameraAdj = (CameraAdjuster) FindObjectOfType(typeof(CameraAdjuster));
		inTest = true;
	}
	public void addRating(float value){
		rating+=value;
	}
	public void evalRarity(){
		stage = (int)rating;
		//All of these are the adjusted values from the system documented
		if(stage==0){
			rarity = new int[]{0,0,0,0,0,0,0,0};
		}else if(stage == 1){
			rarity = new int[]{1,0,0,0,0,0,0,0};
		}else if(stage == 2){
			rarity = new int[]{1,0,0,1,0,0,0,0};
		}else if(stage == 3){
			rarity = new int[]{1,0,0,1,0,0,0,0};
		}else if(stage == 4){
			rarity = new int[]{1,0,0,1,1,0,0,1};
		}else if(stage == 5){
			rarity = new int[]{1,1,0,1,1,0,0,1};
		}else if(stage == 6){
			rarity = new int[]{1,1,1,1,1,0,0,1};
		}else if(stage == 7){
			rarity = new int[]{2,1,1,0,1,1,0,1};
		}else if(stage == 8){
			rarity = new int[]{2,1,1,1,2,0,0,1};
		}else if(stage == 9){
			rarity = new int[]{2,1,2,1,2,0,0,1};
		}else if(stage == 10){
			rarity = new int[]{2,1,2,1,2,1,0,1};
		}else if(stage == 11){
			rarity = new int[]{3,1,2,1,2,1,0,1};
		}else if(stage == 12){
			rarity = new int[]{3,1,2,2,2,1,0,1};
		}else if(stage == 13){
			rarity = new int[]{3,1,2,2,2,1,1,1};
		}else if(stage == 14){
			rarity = new int[]{3,1,2,2,2,1,1,2};
		}else{//15++
			rarity = new int[]{3,1,2,3,2,1,1,2};
		}
		evalPlayer ();
	}
	public void evalPlayer(){
		if (cameraAdj.playerCount == 1) {
			//cut last six slots
			rarity[2] = -1;
			rarity[3] = -1;
			rarity[4] = -1;
			rarity[5] = -1;
			rarity[6] = -1;
			rarity[7] = -1;
		}else if (cameraAdj.playerCount == 2) {
			//cut last four slots
			rarity[4] = -1;
			rarity[5] = -1;
			rarity[6] = -1;
			rarity[7] = -1;
		}else if (cameraAdj.playerCount == 3) {
			//cut last two slots
			rarity[6] = -1;
			rarity[7] = -1;
		}else if (cameraAdj.playerCount == 4) {
			//Clean, all should be the above values already
		}else{//Failure clean (purge all values of loot)
			rarity = new int[]{-1,-1,-1,-1,-1,-1,-1,-1};
		}
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("space")&&inTest){
			DontDestroyOnLoad(transform.gameObject);
			evalRarity();
			Application.LoadLevel("RewardScreen");
		}
		if(Application.loadedLevel==2&&!inTest){
			
		}
	}
}
