// Riot Shield item

using UnityEngine;
using System.Collections;

public class RiotOrRiot : Singular {
	
	private float spdPercent;
	
	public RiotOrRiot(float speedValue) {
		name = "Sprinting";
		spdPercent = speedValue;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
		unit.stats.spdManip.setSpeedAmplification(spdPercent);
	}
	
	public override void removeBD(Character unit) {
		base.removeBD(unit);
		unit.stats.spdManip.removeSpeedAmplification(spdPercent);
	}
	
	public override void purgeBD(Character unit) {
		base.purgeBD (unit);
	}
}

public class RiotShield : ToggleItem {
	
	private float dmgReduction, userSlow;
	private MeshRenderer meshRenderer;

	private Slow debuff;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		meshRenderer = GetComponent<MeshRenderer>();
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 10.0f;
		maxDuration = 5;
		dmgReduction = 0.9f;
		userSlow = 0.75f;
		debuff = new Slow (userSlow);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem();

		// user.animator.SetTrigger("Sprint"); Set speed var in animator once we have the animation
	}

	protected override IEnumerator bgnEffect() {
		collider.enabled = true;
		meshRenderer.enabled = true;
		user.stats.dmgManip.setDamageReduction(1, dmgReduction);
		user.BDS.addBuffDebuff(debuff);
		// user.slow (userSlow);
		return base.bgnEffect();
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void atvDeactivation() {
		collider.enabled = false;
		meshRenderer.enabled = false;

		user.stats.dmgManip.removeDamageReduction(1, dmgReduction);
		user.BDS.rmvBuffDebuff(debuff);
		// user.removeSlow (userSlow);
		base.atvDeactivation();
	}

	protected override void animDone() {
		base.animDone ();
	}
}
