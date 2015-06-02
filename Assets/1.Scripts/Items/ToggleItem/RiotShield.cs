// Riot Shield item

using UnityEngine;
using System.Collections;

public class RiotShield : ToggleItem {
	
	private float dmgReduction, userSlow;
	private Collider col;
	// private MeshRenderer meshRenderer;
	private ParticleSystem particle;

	private FreedomController debuff;

	private class FreedomController : Singular {
		
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

	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		// meshRenderer = GetComponent<MeshRenderer>();
		this.col = this.GetComponent<Collider>();
		this.particle = this.GetComponent<ParticleSystem>();
		this.particle.enableEmission = false;
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 5.0f;
		maxDuration = 5;
		dmgReduction = 1.0f;
		userSlow = 0.6f;
		debuff = new FreedomController (userSlow, dmgReduction);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		this.user.lockRotation = true;
		base.useItem();

		// user.animator.SetTrigger("Sprint"); Set speed var in animator once we have the animation
	}

	protected override IEnumerator bgnEffect() {
		this.col.enabled = true;
		// meshRenderer.enabled = true;
		this.particle.enableEmission = true;
		user.BDS.addBuffDebuff(debuff, user.gameObject);
		return base.bgnEffect();
	}
	
	public override void deactivateItem() {
		this.user.lockRotation = false;
		base.deactivateItem();
	}

	protected override void atvDeactivation() {
		this.col.enabled = false;
		// meshRenderer.enabled = false;
		this.particle.enableEmission = false;
		
		user.BDS.rmvBuffDebuff(debuff, user.gameObject);
		base.atvDeactivation();
	}

	protected override void animDone() {
		base.animDone ();
	}
}
