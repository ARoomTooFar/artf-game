using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerRewardScreen : MonoBehaviour {
	public Transform lootList;
	float newRowYPos;
	Vector2 iconDimens;
	public Sprite icon;
	public RectTransform lootListRect;

	void Start () {
		lootList = transform.Find("LootScroller/ScrollView/LootList");
		lootListRect = lootList.GetComponent<RectTransform>();

		iconDimens = this.GetComponent<RectTransform>().sizeDelta;
//		iconDimens.y = this.GetComponent<RectTransform>().sizeDelta.y;
//		iconDimens.x = this.GetComponent<RectTransform>().sizeDelta.x;

		newRowYPos = 0f - iconDimens.y / 2 + 10f;

		makeNewEntry(newRowYPos, "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
		makeNewEntry(newRowYPos,  "testIcon1");
	}

	void Update () {
		print (lootListRect.rect.height);
	}

	void makeNewEntry(float y, string itemName){
		GameObject newLootItem = Instantiate(Resources.Load("RewardScreen/LootItem")) as GameObject;
		RectTransform lootItemRect = newLootItem.GetComponent<RectTransform>();

		//grow loot list rect to accommodate more entries
		lootListRect.sizeDelta = new Vector2(lootListRect.sizeDelta.x, lootListRect.sizeDelta.y + iconDimens.y - 40f);

		//parent entry to loot list scroll pane
		newLootItem.transform.SetParent(lootList, false);

		//set its position
		lootItemRect.anchoredPosition = new Vector2(0f + iconDimens.x / 2 - 10f, y);

		//give it the proper icon
		icon = Resources.Load<Sprite>("RewardScreen/" + itemName);
		newLootItem.GetComponent<Image>().sprite = icon;

		//increment y position for next iteration
		newRowYPos -= iconDimens.y / 2 + 10f;
	}
	
}
