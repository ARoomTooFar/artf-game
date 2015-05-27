using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SynthKnockBack : QuickItem {

	public List<Character> enemies;
	
	public Enemy eUser;
	
	protected Knockback kb;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.kb = new Knockback(20);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		cooldown = 10.0f;
	}

	// Called when character with an this item selected uses their item key
	public override void useItem() {
		if (this.eUser.target == null) return;
		base.useItem ();
		this.knock ();
		this.animDone ();  
	}
	
	protected override void animDone() {
		base.animDone();
	}

	protected void knock(){
		this.eUser.target.GetComponent<Character>().BDS.addBuffDebuff(this.kb, this.eUser.gameObject, 1);
	}
}
