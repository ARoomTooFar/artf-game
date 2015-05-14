using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewBullyTrunk: NewMobileEnemy {

	public BullyTrunkPummelWeapon rightPaw, leftPaw;

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

	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 175;
		stats.health = stats.maxHealth;
		stats.armor = 7;
		stats.strength = 25;
		stats.coordination=0;
		stats.speed=6;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.5f;
	}

	public override void SetTierData(int tier) {
		tier = 2;
		base.SetTierData (tier);

		monsterLoot.initializeLoot("BullyTrunk", tier);

		this.stats.speed = tier < 3 ? 7 : 10;

		if (tier > 0) {
			charge = this.inventory.items[inventory.selected].GetComponent<BullCharge>();
			if (charge == null) Debug.LogWarning ("BullyTrunk does not have charge equipped");

			foreach(ChargeBehaviour behaviour in this.animator.GetBehaviours<ChargeBehaviour>()) {
				behaviour.SetVar(this.charge);
			}
			
			foreach(ShieldsDown behaviour in this.animator.GetBehaviours<ShieldsDown>()) {
				behaviour.SetVar(this);
			}

			this.charge.chargeSpeed = tier < 5 ? 3 : 4;


			headReduction = 0.9f;
			sideReduction = tier < 3 ? 0.45f : 0.9f;
			headSlow = tier < 6 ? 0.1f : 0.0f;
			sideSlow = tier < 3 ? 0.5f : 0.25f;
			this.rockHead = new RockHead (headSlow, headReduction);
			this.rockArms = new RockArms (sideSlow, sideReduction);
			
			this.BDS.addBuffDebuff (this.rockHead, this.gameObject);
			this.BDS.addBuffDebuff (this.rockArms, this.gameObject);
		}

		if (tier > 1) {
			foreach(Pummel behaviour in this.animator.GetBehaviours<Pummel>()) {
				behaviour.trunk = this.gear.weapon.GetComponent<MeleeWeapons>();
			}
			
			this.rightPaw.equip(this, this.opposition);
			this.leftPaw.equip(this, this.opposition);
		}

		if (tier > 3) {
			this.inventory.cycItems ();
			
			blast = this.inventory.items[inventory.selected].GetComponent<BullyTrunkBlast>();
			if (blast == null) Debug.LogWarning ("BullyTrunk does not have blast equipped");
			
			foreach(TrunkBlast behaviour in this.animator.GetBehaviours<TrunkBlast>()) {
				behaviour.SetVar(this.blast);
			}
		}
	}
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//

	// These are here because for some reason I can't find the inherited collider functions from the animation events thing
	// Fuck Unity, such a tease
	protected virtual void SmashNow() {
		this.AttackStart();
	}

	protected virtual void SmashOver() {
		this.AttackEnd();
	}

	protected virtual void PummelRightNow() {
		this.rightPaw.AttackStart();
	}

	protected virtual void PummelRightOver() {
		this.rightPaw.AttackEnd();
	}

	protected virtual void PummelLeftNow() {
		this.leftPaw.AttackStart();
	}

	protected virtual void PummelLeftOver() {
		this.leftPaw.AttackEnd();
	}

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