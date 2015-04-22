using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerUIPane : MonoBehaviour{
	public Character c;
	public Image hp;
	public Image charge;
	public Image slot1;
	public Image slot2;
	public Image slot3;

	public void initVals(string player){
		c = GameObject.Find(player).GetComponent("Player") as Character;
		hp = transform.Find("HPbar/HP").gameObject.GetComponent<Image>();
		charge = transform.Find("ChargeBar/Charge").gameObject.GetComponent<Image>();
		slot1 = transform.Find("ActionSlots/Slot1/Ring").gameObject.GetComponent<Image>();
		slot2 = transform.Find("ActionSlots/Slot2/Ring").gameObject.GetComponent<Image>();
		slot3 = transform.Find("ActionSlots/Slot3/Ring").gameObject.GetComponent<Image>();
	}

	void Update(){
		hp.fillAmount = ((float)c.stats.health / (float)c.stats.maxHealth);


		slot1.fillAmount = Mathf.MoveTowards(slot1.fillAmount, 1f, Time.deltaTime / 5f);
		if(slot1.fillAmount == 1f) slot1.fillAmount = 0f; //reset to 0 for demo purposes only

		slot2.fillAmount = Mathf.MoveTowards(slot2.fillAmount, 1f, Time.deltaTime / 25f);
		if(slot2.fillAmount == 1f) slot2.fillAmount = 0f; //reset to 0 for demo purposes only

		slot3.fillAmount = Mathf.MoveTowards(slot3.fillAmount, 1f, Time.deltaTime / 15f);
		if(slot3.fillAmount == 1f) slot3.fillAmount = 0f; //reset to 0 for demo purposes only
	}
	
}
