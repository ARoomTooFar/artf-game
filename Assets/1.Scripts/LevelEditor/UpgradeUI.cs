using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
	public GameObject parentObject;

	static UpgradeUI currentActiveObjectUI = null;

	int tier = 0;
	int maxTier = 6;
	Text Text_Tier;
	Text Text_AttackPatterns;
	
	public Camera UICamera;
	public Camera FollowCamera;
	public Canvas UpgradeUICanvas;

	public CameraRaycast camCast;
	public CameraDraws camDraw;
	
	GameObject lowerHalf;
	GameObject tierStats;
	
	Vector3 mouseStartPos;
	
	bool rayHit = false;

	//health, armor, strength, coordination, damage
	static int[,] FoliantFodderStats = new int[6,5]{
		{50, 1, 10, 0, 10}, 
		{100, 3, 20, 0, 20},
		{150, 5, 30, 0, 30},
		{200, 7, 40, 0, 40},
		{250, 9, 50, 0, 50},
		{300, 11, 60, 0, 60}};
	static int[,] FoliantHiveStats = new int[6,5]{
		{200, 5, 5, 0, 0}, 
		{400, 10, 10, 0, 0},
		{600, 15, 15, 0, 0},
		{800, 20, 20, 0, 0},
		{1000, 25, 25, 0, 0},
		{1200, 30, 30, 0, 0}};
	static int[,] CackleBranchStats = new int[6,5]{
		{60, 1, 0, 10, 10}, 
		{120, 2, 0, 20, 20},
		{180, 3, 0, 30, 30},
		{240, 4, 0, 40, 40},
		{300, 5, 0, 50, 50},
		{360, 6, 0, 60, 60}};
	static int[,] BullyTrunkStats = new int[6,5]{
		{175, 7, 25, 0, 10}, 
		{350, 12, 50, 0, 20},
		{525, 17, 75, 0, 30},
		{700, 22, 100, 0, 40},
		{875, 27, 125, 0, 50},
		{1050, 32, 150, 0, 60}};
	static int[,] BushlingStats = new int[6,5]{
		{120, 3, 12, 0, 10}, 
		{240, 6, 24, 0, 20},
		{360, 9, 36, 0, 30},
		{480, 12, 48, 0, 40},
		{600, 15, 60, 0, 50},
		{720, 18, 72, 0, 60}};
	static int[,] ArtillitreeStats = new int[6,5]{
		{200, 15, 0, 60, 10}, 
		{400, 22, 0, 120, 20},
		{600, 29, 0, 180, 30},
		{800, 36, 0, 240, 40},
		{1000, 43, 0, 300, 50},
		{1200, 50, 0, 360, 60}};
	static int[,] MirageStats = new int[6,5]{
		{50, 1, 45, 0, 10}, 
		{100, 3, 90, 0, 20},
		{150, 5, 135, 0, 30},
		{200, 7, 180, 0, 40},
		{250, 9, 225, 0, 50},
		{300, 11, 270, 0, 60}};
	static int[,] SynthStats = new int[6,5]{
		{100, 2, 0, 3, 10}, 
		{200, 5, 0, 6, 20},
		{300, 8, 0, 9, 30},
		{400, 11, 0, 12, 40},
		{500, 14, 0, 15, 50},
		{600, 17, 0, 18, 60}};
	static int[,] DartPlantStats = new int[6,5]{
		{50, 1, 10, 0, 10}, 
		{100, 3, 20, 0, 20},
		{150, 5, 30, 0, 30},
		{200, 7, 40, 0, 40},
		{250, 9, 50, 0, 50},
		{300, 11, 60, 0, 60}};

	//for updating stat value text on ui panel
	Dictionary<string, int[,]> MonsterStats = new Dictionary<string, int[,]>(){
		{"FoliantFodder", FoliantFodderStats},
		{"FoliantHive", FoliantHiveStats},
		{"CackleBranch", CackleBranchStats},
		{"BullyTrunk", BullyTrunkStats},
		{"Bushling", BushlingStats},
		{"Artillitree", ArtillitreeStats},
		{"Mirage", MirageStats},
		{"Synth", FoliantFodderStats},
		{"DartPlant", DartPlantStats}

	};



	void Start ()
	{
		Button btn;
		btn = transform.Find("LowerHalf/Button_TierUp").GetComponent<Button>();
		btn.onClick.AddListener (() => {
			increaseTier (); });

		btn = transform.Find("LowerHalf/Button_TierDown").GetComponent<Button>();
		btn.onClick.AddListener (() => {
			decreaseTier (); });

		btn = transform.Find("LowerHalf/Button_Rotate").GetComponent<Button>();
		btn.onClick.AddListener (() => {
			MapData.rotateObject(this.transform.parent.gameObject, this.transform.parent.gameObject.transform.position);
		});

		btn = transform.Find("LowerHalf/Button_X").GetComponent<Button>();
		btn.onClick.AddListener (() => {
			toggleUpgradeUI(); 
		});

		parentObject = transform.parent.gameObject;

		Text_Tier = transform.Find("LowerHalf/Text_Tier").GetComponent<Text>();
		updateMonsterStatText ();
		
		UpgradeUICanvas = this.gameObject.GetComponent<Canvas>();

		UICamera = GameObject.Find("UpgradeUICamera").GetComponent<Camera>();
		FollowCamera = GameObject.Find("UICamera").GetComponent<Camera>();

		camCast = FollowCamera.GetComponent<CameraRaycast>();
		camDraw = FollowCamera.GetComponent<CameraDraws>();

		//sets Render Camera if in Screen Space - Camera mode
		//sets World Camera if in World Space mode
		UpgradeUICanvas.worldCamera = UICamera;
		
		//ui elements we need to toggle on click
		lowerHalf = transform.Find("LowerHalf").gameObject;
		tierStats = transform.Find("TierStats").gameObject;
		
		//initialize UI to be invisible
		lowerHalf.SetActive(false);
		tierStats.SetActive(false);

		mouseStartPos = new Vector3(0,0,0);

		updateTiers();
	}
	
	//Update causes itemObjectUI flickering. LateUpdate prevents it
	void LateUpdate(){
		faceUIToCamera();

		if(FollowCamera.orthographic == true){
			lowerHalf.SetActive(false);
			tierStats.SetActive(false);
			currentActiveObjectUI = null;
		}
	}

	//updates tier-related info
	void updateTiers(){
		//turn tiers gray that aren't applied. turn tiers green that are applied
		foreach(Transform thing in tierStats.transform) {
			if(thing.name.Contains("Tier")){
				if(Convert.ToInt32(thing.name[thing.name.Length-1])-Convert.ToInt32('0') <= tier){
					thing.GetComponent<Text>().color = Color.green;
				} else {
					thing.GetComponent<Text>().color = Color.gray;
				}
			}

			//if it has a (Clone) in the name (which it probahly should), get rid of
			//the clone text
			string monsterName = this.parentObject.gameObject.name;
			if(monsterName.Contains("(Clone)")){
				monsterName = monsterName.Substring(0, monsterName.IndexOf("("));
			}

			//update stat values
			if(thing.name.Contains("StatValues") && tier < maxTier && monsterName != "Copy"){
				thing.GetComponent<Text>().text = 
						"Health: " + MonsterStats[monsterName][tier,0].ToString() +
						"\nArmor: " + MonsterStats[monsterName][tier,1].ToString() + "%" +
						"\nStrength: " + MonsterStats[monsterName][tier,2].ToString() +
						"\nCoordination: " + MonsterStats[monsterName][tier,3].ToString()/* +
						"\nDamage: " + MonsterStats[monsterName][tier,4].ToString()*/;
			}
		}
	}

	void scaleWorldSpaceCanvas(){
		float uiScaleFactor = FollowCamera.transform.position.y / 17f;

		UpgradeUICanvas.GetComponent<RectTransform>().localScale = 
			new Vector3(uiScaleFactor, uiScaleFactor, 1f);
	}

	void stickScreenSpaceOverlayCameraToObject(){
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(FollowCamera, parentObject.transform.position);
		UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition = screenPoint;
	}

	void Update(){

		//keeps canvas stuck on world object. for when canvas is in Screen Space - Overlay mode
//		stickScreenSpaceOverlayCameraToObject();

		//resizes canvas according to camera zoom, for when canvas is in World Space mode
		scaleWorldSpaceCanvas();

		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			if(camCast.hitDistance != Mathf.Infinity
			   && camCast.hit.collider.gameObject.GetInstanceID() == parentObject.gameObject.GetInstanceID()){
				rayHit = true;
				mouseStartPos = Input.mousePosition;
			}
		}
		
		//checks if a drag has been performed. drags count as clicks.
		//if drag, toggle UI to turn it to its original state
		if (Input.GetMouseButtonUp(0) && rayHit == true){ 
			Vector3 offset = Input.mousePosition - mouseStartPos;
			
			//if the offset is not zero, then a drag happened
			if (Math.Abs(offset.x) == 0){
				toggleUpgradeUI();
			}
			rayHit = false;
		}
	}
	
	void faceUIToCamera(){
		UpgradeUICanvas.transform.position = parentObject.transform.position;
		UpgradeUICanvas.transform.rotation = UICamera.transform.rotation;
	}

	public void increaseTier ()
	{
		tier = Math.Min(tier + 1, maxTier);
		updateMonsterStatText ();
		parentObject.GetComponent<MonsterData>().Tier = tier;
		updateTiers();
	}
	
	public void decreaseTier ()
	{
		tier = Math.Max(0, tier - 1);
		updateMonsterStatText ();
		parentObject.GetComponent<MonsterData>().Tier = tier;
		updateTiers();
	}

	//Update the monster stat text with the newly changed value
	void updateMonsterStatText ()
	{
		Text_Tier.text = "Tier: " + tier.ToString ();
	}
	
	//makes sure only one ObjectUI is active in the level editor
	public bool toggleUpgradeUI(){
		if(currentActiveObjectUI == null){
			lowerHalf.SetActive(true);
			tierStats.SetActive(true);
			currentActiveObjectUI = this.gameObject.GetComponent<UpgradeUI>();
		}else if(currentActiveObjectUI == this.gameObject.GetComponent<UpgradeUI>()){
			lowerHalf.SetActive(false);
			tierStats.SetActive(false);
			currentActiveObjectUI = null;
		}else{
			currentActiveObjectUI.toggleUpgradeUI();
			lowerHalf.SetActive(true);
			tierStats.SetActive(true);
			currentActiveObjectUI = this.gameObject.GetComponent<UpgradeUI>();
		}

		//if already active return false, otherwise return true (used for select/deselect all)
		if (!lowerHalf.activeSelf)
			return false;
		else 
			return true;
	}
}
