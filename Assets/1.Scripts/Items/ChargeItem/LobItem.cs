using UnityEngine;
using System.Collections;

public class LobItem : ChargeItem {
	public GameObject targetCircle;
	public TargetCircle curCircle;
	public GameObject projectile;
	protected ArcingBomb bullet;
	public Immobilize immobil;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		immobil = new Immobilize();
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 10.0f;
		curChgTime = -1.0f;
		maxChgTime = 2.0f;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		//base.useItem();
		if (cdBar != null) {
			cdBar.onState = 2;
			cdBar.max = 3;
		}
		curChgTime = 0.0f;
		StartCoroutine(bgnCharge());
	}
	/*
	// If things need to be done while charging make this virtual 
	protected IEnumerator bgnCharge() {
		curChgTime = 0.0f;
		while (user.inventory.keepItemActive) {
			curChgTime = Mathf.Clamp(curChgTime + Time.deltaTime, 0.0f, maxChgTime);
			if (cdBar != null) cdBar.current = curChgTime + Time.deltaTime;
			yield return null;
		}
		if (cdBar != null) cdBar.current = curChgTime;
		deactivateItem();
	}*/
	
	public override void deactivateItem() {
		base.deactivateItem();
		this.fireProjectile();
		StartCoroutine(itemFinish());
		if (curChgTime >= 0.0f) {
			chgDone();
		}
	}

	protected override void animDone() {
		user.freeAnim = true;
		curCoolDown = cooldown + (curChgTime * 3);
		if (cdBar != null) {
			cdBar.onState = 1;
			cdBar.max = curCoolDown;
		}
		curChgTime = -1.0f;

		base.animDone ();
	}
	protected virtual IEnumerator itemFinish() {
		while (user.inventory.keepItemActive) {
			yield return null;
		}
		user.BDS.rmvBuffDebuff(immobil, this.gameObject);
		user.testControl = true;
	}

	// Once key to charge is released (Or other stuff like taking damage), do what are ability is
	protected override void chgDone() {
		animDone();
	}
	protected override IEnumerator bgnCharge() {
		user.testControl = false;
		user.BDS.addBuffDebuff(immobil,this.gameObject);
		Vector3 direction = user.facing.normalized * 4.0f;
		// this.curTCircle.transform.position = new Vector3(this.curTCircle.transform.position.x + direction.x, this.curTCircle.transform.position.y, this.curTCircle.transform.position.z + direction.z);
		curCircle = ((GameObject)Instantiate(targetCircle, new Vector3(user.transform.position.x + direction.x, 0.55f, user.transform.position.z + direction.z), user.transform.rotation)).GetComponent<TargetCircle>();
		curCircle.setValues (this.user);
		while (user.inventory.keepItemActive) {
			curChgTime = Mathf.Clamp(curChgTime + Time.deltaTime, 0.0f, maxChgTime);
			yield return null;
		}
		user.animator.SetBool ("Charging", false);
		deactivateItem();
	}
	protected void fireProjectile() {
		this.bullet = ((GameObject)Instantiate(projectile, user.transform.position, user.transform.rotation)).GetComponent<ArcingBomb>();
		this.bullet.setInitValues(user, opposition, 0, false, null, this.curCircle.gameObject);
		this.curCircle.moveable = false;
		//this.curCircle = null;
	}
}
