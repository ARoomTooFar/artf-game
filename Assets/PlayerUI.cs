using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Character p1;

	public LifeBar p1lifebar;
	public LifeBar p2lifebar;
	public LifeBar p3lifebar;
	public LifeBar p4lifebar;

	public Image p1hp;
	public Image p2hp;
	public Image p3hp;
	public Image p4hp;

	public Image p1charge;
	public Image p2charge;
	public Image p3charge;
	public Image p4charge;

	public Image p1actionslot1;
	public Image p1actionslot2;
	public Image p1actionslot3;

	public Image p2actionslot1;
	public Image p2actionslot2;
	public Image p2actionslot3;

	public Image p3actionslot1;
	public Image p3actionslot2;
	public Image p3actionslot3;

	public Image p4actionslot1;
	public Image p4actionslot2;
	public Image p4actionslot3;


	void Start () {

		p1 = GameObject.Find("PlayerZ").GetComponent("Player") as Player;

		p1hp = transform.Find("Player1/HPbar/HP").gameObject.GetComponent<Image>();
		p2hp = transform.Find("Player2/HPbar/HP").gameObject.GetComponent<Image>();
		p3hp = transform.Find("Player3/HPbar/HP").gameObject.GetComponent<Image>();
		p4hp = transform.Find("Player4/HPbar/HP").gameObject.GetComponent<Image>();

		p1charge = transform.Find("Player1/ChargeBar/Charge").gameObject.GetComponent<Image>();
		p2charge = transform.Find("Player2/ChargeBar/Charge").gameObject.GetComponent<Image>();
		p3charge = transform.Find("Player3/ChargeBar/Charge").gameObject.GetComponent<Image>();
		p4charge = transform.Find("Player4/ChargeBar/Charge").gameObject.GetComponent<Image>();

		p1actionslot1 = transform.Find("Player1/ActionSlots/Slot1/Ring").gameObject.GetComponent<Image>();
		p1actionslot2 = transform.Find("Player1/ActionSlots/Slot2/Ring").gameObject.GetComponent<Image>();
		p1actionslot3 = transform.Find("Player1/ActionSlots/Slot3/Ring").gameObject.GetComponent<Image>();

		p2actionslot1 = transform.Find("Player2/ActionSlots/Slot1/Ring").gameObject.GetComponent<Image>();
		p2actionslot2 = transform.Find("Player2/ActionSlots/Slot2/Ring").gameObject.GetComponent<Image>();
		p2actionslot3 = transform.Find("Player2/ActionSlots/Slot3/Ring").gameObject.GetComponent<Image>();

		p3actionslot1 = transform.Find("Player3/ActionSlots/Slot1/Ring").gameObject.GetComponent<Image>();
		p3actionslot2 = transform.Find("Player3/ActionSlots/Slot2/Ring").gameObject.GetComponent<Image>();
		p3actionslot3 = transform.Find("Player3/ActionSlots/Slot3/Ring").gameObject.GetComponent<Image>();

		p4actionslot1 = transform.Find("Player4/ActionSlots/Slot1/Ring").gameObject.GetComponent<Image>();
		p4actionslot2 = transform.Find("Player4/ActionSlots/Slot2/Ring").gameObject.GetComponent<Image>();
		p4actionslot3 = transform.Find("Player4/ActionSlots/Slot3/Ring").gameObject.GetComponent<Image>();
	}
	

	void Update () {
		p1hp.fillAmount = ((float)p1.stats.health / (float)p1.stats.maxHealth);


	}
	


}
