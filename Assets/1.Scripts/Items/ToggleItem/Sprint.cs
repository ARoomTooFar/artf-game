// Sprint item

using UnityEngine;
using System.Collections;

public class Sprint : ToggleItem {

	[Range(1.5f, 3.0f)]
	public float sprintAmplification;
	//private int baseSpeed;

	private Sprinting buff;

	private class Sprinting : Singular {
		
		private float spdPercent;
		
		public Sprinting(float speedValue) {
			name = "Sprinting";
			spdPercent = speedValue/2;
		}
		
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			newData.unit.stats.spdManip.setSpeedAmplification(spdPercent);
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.stats.spdManip.removeSpeedAmplification(spdPercent);
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		buff = new Sprinting(sprintAmplification);
	}

	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 10.0f;
		maxDuration = 5;
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
		//baseSpeed = user.stats.speed;

		user.BDS.addBuffDebuff(buff, user.gameObject);
		// user.speed(sprintAmplification);

		return base.bgnEffect();
	}

	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void atvDeactivation() {
		user.BDS.rmvBuffDebuff(buff, user.gameObject);
		// user.removeSpeed(sprintAmplification);

		base.atvDeactivation();
	}

	protected override void animDone() {
		base.animDone ();
	}
}
