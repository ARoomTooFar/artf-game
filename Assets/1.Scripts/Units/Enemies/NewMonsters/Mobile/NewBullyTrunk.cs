using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewBullyTrunk: NewMobileEnemy {

	protected BullCharge charge;
	protected BullyTrunkBlast blast;
	protected RockHead rockHead;
	protected RockArms rockArms;
	
	protected float headReduction, sideReduction, headSlow, sideSlow;
	
	
	// The front and side versions of riot shield for the bully trunk (Differing values and directional effects
	protected class RockHead : Singular {
		
		private float spdPercent, redPercent;
		
		public RockHead(float speedValue, float reduxValue) {
			name = "RockHead";
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
	
	protected class RockArms : Singular {
		private float spdPercent, redPercent;
		
		public RockArms(float speedValue, float reduxValue) {
			name = "RockArms";
			spdPercent = speedValue;
			redPercent = reduxValue;
		}
		
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			newData.unit.stats.dmgManip.setDamageReduction(3, redPercent);
			newData.unit.stats.spdManip.setSpeedReduction(spdPercent);
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.stats.dmgManip.removeDamageReduction(3, redPercent);
			oldData.unit.stats.spdManip.removeSpeedReduction(spdPercent);
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}

	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();

		charge = this.inventory.items[inventory.selected].GetComponent<BullCharge>();
		if (charge == null) Debug.LogWarning ("BullyTrunk does not have charge equipped");
		
		this.inventory.cycItems ();
		
		blast = this.inventory.items[inventory.selected].GetComponent<BullyTrunkBlast>();
		if (blast == null) Debug.LogWarning ("BullyTrunk does not have blast equipped");

		foreach(Charge behaviour in this.animator.GetBehaviours<Charge>()) {
			behaviour.charge = this.charge;
		}

		foreach(BullyApproach behaviour in this.animator.GetBehaviours<BullyApproach>()) {
			behaviour.charge = this.charge;
		}

		foreach(Ram behaviour in this.animator.GetBehaviours<Ram>()) {
			behaviour.charge = this.charge;
			behaviour.blast = this.blast;
		}
		
		headReduction = 0.9f;
		sideReduction = 0.45f;
		headSlow = 0.1f;
		sideSlow = 0.5f;
		this.rockHead = new RockHead (headSlow, headReduction);
		this.rockArms = new RockArms (sideSlow, sideReduction);
		
		this.BDS.addBuffDebuff (this.rockHead, this.gameObject);
		this.BDS.addBuffDebuff (this.rockArms, this.gameObject);
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//
	
	//-------------------//
	
	
	//------------//
	// Coroutines //
	//------------//

	public virtual IEnumerator shieldsDown() {
		this.BDS.rmvBuffDebuff (this.rockArms, this.gameObject);
		yield return new WaitForSeconds(3.0f);
		this.BDS.addBuffDebuff (this.rockArms, this.gameObject);
	}

	
	//------------//
	
	
	//------------------//
	// Helper Functions //
	//------------------//
	
	
	//------------------//
}