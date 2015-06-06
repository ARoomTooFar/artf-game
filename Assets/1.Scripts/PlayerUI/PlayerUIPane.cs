using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//handles a single player pane
public class PlayerUIPane : MonoBehaviour
{
	public Player pl;
	public Image hp;
	public Image greyhp;
	public Image charge;
	public Image slot1;
	public Image slot2;
	public Image slot3;
	public Image slot1bg;
	public Image slot2bg;
	public Image slot3bg;
	public int currSlot = 1;
	float slot1Curr,slot2Curr,slot3Curr,/*chgCurr,*/tempCD1,tempCD2,tempCD3;

	public void initVals (string player)
	{
		pl = GameObject.FindGameObjectWithTag(player).GetComponent ("Player") as Player;
		hp = transform.Find ("HP").gameObject.GetComponent<Image> ();
		greyhp = transform.Find ("GreyHP").gameObject.GetComponent<Image> ();
		slot1 = transform.Find ("Ring1").gameObject.GetComponent<Image> ();
		slot2 = transform.Find ("Ring1").gameObject.GetComponent<Image> ();
		slot3 = transform.Find ("Ring1").gameObject.GetComponent<Image> ();

		slot1bg = transform.Find ("Ring1BG").gameObject.GetComponent<Image> ();
		slot2bg = transform.Find ("Ring2BG").gameObject.GetComponent<Image> ();
		slot3bg = transform.Find ("Ring3BG").gameObject.GetComponent<Image> ();


		slot1.fillAmount = 1;
		slot2.fillAmount = 1;
		slot3.fillAmount = 1;

		print (pl.inventory.items[0]);
		print (pl.inventory.items[1]);
		print (pl.inventory.items[2]);
		setAbilityIcons();
	}

	void setAbilityIcons(){
		slot1bg.sprite = Resources.Load <Sprite>("PlayerUI/HUDV2Assets/" + pl.inventory.items[0]);
		slot2bg.sprite = Resources.Load <Sprite>("PlayerUI/HUDV2Assets/" + pl.inventory.items[1]);
		slot3bg.sprite = Resources.Load <Sprite>("PlayerUI/HUDV2Assets/" + pl.inventory.items[2]);
	}

	void Update ()
	{
		updateHPBar ();
		updateSlots ();
		updateChg ();
	}

	void updateHPBar ()
	{
		hp.fillAmount = normalizeForFilling (pl.stats.health);
		greyhp.fillAmount = normalizeForFilling (pl.stats.health) + normalizeForFilling (pl.greyDamage);

		if (pl.isDead) {
			hp.fillAmount = 0f;
			greyhp.fillAmount = 0f;
		}
	}
	void updateChg(){
		if (pl.inventory.keepItemActive && pl.inventory.items [pl.inventory.selected].itemType == 'C') {
			if ((pl.inventory.items [pl.inventory.selected].GetComponent<ChargeItem> ().curChgTime / pl.inventory.items [pl.inventory.selected].GetComponent<ChargeItem> ().maxChgTime) <= 0) {
				//chgCurr = 0;
			} else {
				//chgCurr = (pl.inventory.items [pl.inventory.selected].GetComponent<ChargeItem> ().curChgTime / pl.inventory.items [pl.inventory.selected].GetComponent<ChargeItem> ().maxChgTime);
			}
		} 
	}

	void updateSlots ()
	{
		currSlot = pl.inventory.selected + 1;
		switch (currSlot) {
		case 1:
			slot1bg.color = Color.yellow;
			slot2bg.color = Color.gray;
			slot3bg.color = Color.gray;
			break;
		case 2:
			slot1bg.color = Color.gray;
			slot2bg.color = Color.yellow;
			slot3bg.color = Color.gray;
			break;
		case 3:
			slot1bg.color = Color.gray;
			slot2bg.color = Color.gray;
			slot3bg.color = Color.yellow;
			break;
		}if ((pl.inventory.items [0].curCoolDown / pl.inventory.items [0].cooldown) <= 0) {
			slot1Curr = 1;
			tempCD1 = 0;
		} else if ((pl.inventory.items [0].curCoolDown > pl.inventory.items [0].cooldown)) {
			if(pl.inventory.items[0].curCoolDown > tempCD1){
				tempCD1 = pl.inventory.items [0].curCoolDown;
			}
			slot1Curr = (pl.inventory.items [0].curCoolDown / tempCD1);
		} else {
			if(tempCD1 > 0){
				slot1Curr = (pl.inventory.items [0].curCoolDown / tempCD1);
			}else{
				slot1Curr = (pl.inventory.items [0].curCoolDown / pl.inventory.items [0].cooldown);
			}
		}
		slot1.fillAmount = Mathf.MoveTowards (slot1.fillAmount, slot1Curr,1f* Time.deltaTime);
		if ((pl.inventory.items [1].curCoolDown / pl.inventory.items [1].cooldown) <= 0) {
			slot2Curr = 1;
			tempCD2 = 0;
		} else if ((pl.inventory.items [1].curCoolDown > pl.inventory.items [1].cooldown)) {
			if(pl.inventory.items[1].curCoolDown > tempCD2){
				tempCD2 = pl.inventory.items [1].curCoolDown;
			}
			slot2Curr = (pl.inventory.items [1].curCoolDown / tempCD2);
		} else {
			if(tempCD2 > 0){
				slot2Curr = (pl.inventory.items [1].curCoolDown / tempCD2);
			}else{
				slot2Curr = (pl.inventory.items [1].curCoolDown / pl.inventory.items [1].cooldown);
			}
		}
		slot2.fillAmount = Mathf.MoveTowards (slot2.fillAmount, slot2Curr, 1f* Time.deltaTime);
		if ((pl.inventory.items [2].curCoolDown / pl.inventory.items [2].cooldown) <= 0) {
			slot3Curr = 1;
			tempCD3 = 0;
		} else if ((pl.inventory.items [2].curCoolDown > pl.inventory.items [2].cooldown)) {
			if(pl.inventory.items[2].curCoolDown > tempCD3){
				tempCD3 = pl.inventory.items [2].curCoolDown;
			}
			slot3Curr = (pl.inventory.items [2].curCoolDown / tempCD3);
		} else {
			if(tempCD3 > 0){
				slot3Curr = (pl.inventory.items [2].curCoolDown / tempCD3);
			}else{
				slot3Curr = (pl.inventory.items [2].curCoolDown / pl.inventory.items [2].cooldown);
			}
		}

		slot3.fillAmount = Mathf.MoveTowards (slot3.fillAmount, slot3Curr, 1f* Time.deltaTime);
	}

	float normalizeForFilling (int val)
	{
		return ((float)val / (float)pl.stats.maxHealth);
	}

	float normalizeForFilling (float val)
	{
		return (val / (float)pl.stats.maxHealth);
	}
}
