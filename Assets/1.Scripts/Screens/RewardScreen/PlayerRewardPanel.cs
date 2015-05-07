using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//controls reward panel for each player
public class PlayerRewardPanel : MonoBehaviour {
	public Transform lootList;
	public RectTransform lootListRect;
	float newRowYPos;
	Vector2 iconDimens;
	public Sprite icon;
	List<string> loot;
	public List<GameObject> highlights;
	public int activeEntry;
	public RectTransform scrollView;

	void Start () {
		lootList = transform.Find("LootScroller/ScrollView/LootList");
		lootListRect = lootList.GetComponent<RectTransform>();
		loot = new List<string>();
		highlights = new List<GameObject>();

		//hardcoded for now
		loot.Add("testIcon1");
		loot.Add("testIcon1");
		loot.Add("testIcon1");
		loot.Add("testIcon1");
		loot.Add("testIcon1");
		loot.Add("testIcon1");
		loot.Add("testIcon1");
		loot.Add("testIcon1");

		//spawn an icon just to get dimensions, then destroy it
		GameObject newLootItem = Instantiate(Resources.Load("RewardScreen/LootEntry")) as GameObject;
		iconDimens = newLootItem.GetComponent<RectTransform>().sizeDelta;
		Destroy (newLootItem);

		//populate list with looted items
		newRowYPos = 0f - iconDimens.y / 2 - 5f;
//		newRowYPos = 0f;
		for(int i = 0; i < loot.Count; i++){
			makeNewEntry(loot[i]);
			highlights[i].SetActive(false);
		}
		highlights[0].SetActive(true);
		activeEntry = 0;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			if(activeEntry < highlights.Count - 1)
				activeEntry += 1;
		}else if(Input.GetKeyDown(KeyCode.UpArrow)){
			if(activeEntry > 0)
				activeEntry -= 1;
		}

		for(int i = 0; i < highlights.Count; i++){
			if(i != activeEntry){
				highlights[i].SetActive(false);
			}else{
				highlights[i].SetActive(true);
			}
		}
	}

	void makeNewEntry(string itemName){
		GameObject newLootItem = Instantiate(Resources.Load("RewardScreen/LootEntry")) as GameObject;
		RectTransform lootItemRect = newLootItem.GetComponent<RectTransform>();

		//grow loot list rect to accommodate more entries
		lootListRect.sizeDelta = new Vector2(lootListRect.sizeDelta.x, lootListRect.sizeDelta.y + iconDimens.y);

		//parent entry to loot list scroll pane
		newLootItem.transform.SetParent(lootList, false);

		//set entry's position
		lootItemRect.anchoredPosition = new Vector2(0f + iconDimens.x / 2, newRowYPos);

		//give it the proper icon
		icon = Resources.Load<Sprite>("RewardScreen/" + itemName);
		newLootItem.transform.Find("Icon").GetComponent<Image>().sprite = icon;

		//add entry's height to list for highlighting
		highlights.Add(newLootItem.transform.Find("Selector").gameObject);

		//increment y position for next iteration
		newRowYPos -= iconDimens.y;
	}
	
}
