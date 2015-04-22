using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//handles a single player pane
public class PlayerUIPane : MonoBehaviour{
	public Player pl;
	public Image hp;
	public Image greyhp;
	public Image charge;
	public Image slot1;
	public Image slot2;
	public Image slot3;

	public void initVals(string player){
		pl = GameObject.Find(player).GetComponent("Player") as Player;
		hp = transform.Find("HPbar/HP").gameObject.GetComponent<Image>();
		greyhp = transform.Find("HPbar/GreyHP").gameObject.GetComponent<Image>();
		charge = transform.Find("ChargeBar/Charge").gameObject.GetComponent<Image>();
		slot1 = transform.Find("ActionSlots/Slot1/Ring").gameObject.GetComponent<Image>();
		slot2 = transform.Find("ActionSlots/Slot2/Ring").gameObject.GetComponent<Image>();
		slot3 = transform.Find("ActionSlots/Slot3/Ring").gameObject.GetComponent<Image>();
	}

	void Update(){
		hp.fillAmount = normalizeForFilling(pl.stats.health);
		greyhp.fillAmount = normalizeForFilling(pl.stats.health) + normalizeForFilling(pl.greyDamage);
		
		slot1.fillAmount = Mathf.MoveTowards(slot1.fillAmount, 1f, Time.deltaTime / 5f);
		if(slot1.fillAmount == 1f) slot1.fillAmount = 0f; //reset to 0 for demo purposes only

		slot2.fillAmount = Mathf.MoveTowards(slot2.fillAmount, 1f, Time.deltaTime / 25f);
		if(slot2.fillAmount == 1f) slot2.fillAmount = 0f; //reset to 0 for demo purposes only

		slot3.fillAmount = Mathf.MoveTowards(slot3.fillAmount, 1f, Time.deltaTime / 15f);
		if(slot3.fillAmount == 1f) slot3.fillAmount = 0f; //reset to 0 for demo purposes only
	}

	float normalizeForFilling(int val){
		return ((float)val /  (float)pl.stats.maxHealth);
	}

	float normalizeForFilling(float val){
		return (val /  (float)pl.stats.maxHealth);
	}
}
