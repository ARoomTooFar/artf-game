using UnityEngine;
using System.Collections;

public class SlowField : Traps {

	private float slowPercent;

	private SlowingField debuff;

	// Slowing field specific debuff that slows units in range.
	//     Singular buff, so that having overlapping traps won't make players move like CM
	private class SlowingField : Singular {
		
		private float spdPercent;
		
		public SlowingField(float speedValue) {
			name = "SlowField";
			spdPercent = speedValue;
		}
		
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			newData.unit.stats.spdManip.setSpeedReduction(spdPercent);
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.stats.spdManip.removeSpeedReduction(spdPercent);
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues ();

		slowPercent = 0.35f;

		debuff = new SlowingField (slowPercent);
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
	
	// A simple enter
	void OnTriggerEnter(Collider other) {
		Character enemy = other.GetComponent<Character>();
		if (enemy != null) {
			enemy.BDS.addBuffDebuff(debuff, this.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		Character enemy = other.GetComponent<Character>();
		if (enemy != null) {
			enemy.BDS.rmvBuffDebuff(debuff, this.gameObject);
		}
	}
}
