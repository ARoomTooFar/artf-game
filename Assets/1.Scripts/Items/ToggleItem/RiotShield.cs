// Riot Shield item

using UnityEngine;
using System.Collections;

public class FreedomController : Singular {
	
	private float spdPercent, redPercent;
	
	public FreedomController(float speedValue, float reduxValue) {
		name = "FreedomControl";
		spdPercent = speedValue;
		redPercent = reduxValue;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.stats.dmgManip.setDamageReduction(1, redPercent);
		newData.unit.stats.spdManip.setSpeedReduction(spdPercent);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.dmgManip.removeDamageReduction(1, redPercent);
		oldData.unit.stats.spdManip.removeSpeedReduction(spdPercent);
	}
	
	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}

public class RiotShield : ToggleItem {
	
	private float dmgReduction, userSlow;
	private MeshRenderer meshRenderer;

	private FreedomController debuff;

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
		debuff = new FreedomController (userSlow, dmgReduction);
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
		user.BDS.addBuffDebuff(debuff, user.gameObject);
		return base.bgnEffect();
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void atvDeactivation() {
		collider.enabled = false;
		meshRenderer.enabled = false;
		
		user.BDS.rmvBuffDebuff(debuff, user.gameObject);
		base.atvDeactivation();
	}

	protected override void animDone() {
		base.animDone ();
	}
}
