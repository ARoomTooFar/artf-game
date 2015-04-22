using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewBushman : NewMobileEnemy {
	
	private class BMFrenzyGrowth : StatsMultiplier {
		protected StatsMultiplier FrenzyGrowth(float dmgUp, float dmgRedUp, float spdUp, float currentGrowth) {
			StatsMultiplier nextLv = new StatsMultiplier ();
			nextLv.dmgAmp = dmgUp * currentGrowth;
			nextLv.dmgRed = dmgRedUp * currentGrowth;
			nextLv.speed = spdUp * currentGrowth;
			return nextLv;
		}
	}

	protected GameObject oldTarget; // Needs to know when it switches targets

	protected Sprint sprint;

	// Old Stuff
	protected PowerLevels powlvs;
	protected Frenzy frenzy;
	protected float health;

	protected BullCharge charge;
	protected Roll lungeAttack;


	//-------------------//
	// Primary Functions //
	//-------------------//

	protected override void Awake () {
		base.Awake ();

	}
	
	protected override void Start() {
		base.Start ();
		setFrenzy ();

		// charge = this.inventory.items[inventory.selected].GetComponent<BullCharge>();
		// lungeAttack = this.inventory.items[++inventory.selected].GetComponent<Roll>();
		// if (charge == null) Debug.LogWarning ("Bushmen does not have charge");
	}

	protected override void Update() {
		if(health > stats.health){
			health = stats.health;
			powlvs.addRage(Mathf.CeilToInt((float)(stats.maxHealth - stats.health)/stats.maxHealth * 150));
		}
		powlvs.Update();
		base.Update ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=6;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 2.0f;
	}

	public override void SetTierData(int tier) {
		tier = 3;
		base.SetTierData (tier);


		if (tier > 0) {
			this.sprint = this.inventory.items[inventory.selected].GetComponent<Sprint>();
			if (sprint == null) Debug.LogWarning ("Bushman does not have sprint equipped");
			

			// blast = this.inventory.items[inventory.selected].GetComponent<BullyTrunkBlast>();
			// if (blast == null) Debug.LogWarning ("BullyTrunk does not have blast equipped");
			
			foreach(SprintBehaviour behaviour in this.animator.GetBehaviours<SprintBehaviour>()) {
				behaviour.SetVar(this.sprint);
			}
		}

		if (tier > 2) {
			this.inventory.cycItems ();

			this.charge = this.inventory.items[inventory.selected].GetComponent<BullCharge>();
			if (charge == null) Debug.LogWarning ("Bushman does not have BullCharge equipped");

			foreach(ChargeBehaviour behaviour in this.animator.GetBehaviours<ChargeBehaviour>()) {
				behaviour.SetVar(this.charge);
			}
		}
	}

	protected override void TargetFunction() {
		base.TargetFunction();
		if (this.oldTarget != null && this.target != this.oldTarget) this.animator.SetTrigger("TargetSwitched");
	}

	//----------------------------------//

	protected void setFrenzy() {
		powlvs = new PowerLevels (this);
		StatsMultiplier stage0 = new StatsMultiplier (); stage0.dmgAmp = 0f; stage0.dmgRed = 0f; stage0.speed = 0f;
		StatsMultiplier stage1 = new StatsMultiplier (); stage1.dmgAmp = 0.2f; stage1.dmgRed = -0.1f; stage1.speed = 0.1f;
		StatsMultiplier stage2 = new StatsMultiplier (); stage2.dmgAmp = 0.3f; stage2.dmgRed = -0.25f; stage2.speed = 0.2f;
		StatsMultiplier stage3 = new StatsMultiplier (); stage3.dmgAmp = 0.36f; stage3.dmgRed = -0.3f; stage3.speed = 0.25f;
		StatsMultiplier stage4 = new StatsMultiplier (); stage4.dmgAmp = 0.5f; stage4.dmgRed = -0.35f; stage4.speed = 0.4f;
		StatsMultiplier stage5 = new StatsMultiplier (); stage5.dmgAmp = 0.75f; stage5.dmgRed = -0.4f; stage5.speed = 0.6f;
		StatsMultiplier stage6 = new StatsMultiplier (); stage6.dmgAmp = 1.0f; stage6.dmgRed = -0.8f; stage6.speed = 1.2f;
		powlvs.addStage(stage0, 0);
		powlvs.addStage(stage1, 10);
		powlvs.addStage(stage2, 20);
		powlvs.addStage(stage3, 25);
		powlvs.addStage(stage4, 36);
		powlvs.addStage(stage5, 45);
		powlvs.addStage(stage6, 60);
	}
	
	// all floats represent percent increase
	protected StatsMultiplier FrenzyGrowth(float dmgUp, float dmgRedUp, float spdUp, float currentGrowth) {
		StatsMultiplier nextLv = new StatsMultiplier ();
		nextLv.dmgAmp = dmgUp * currentGrowth;
		nextLv.dmgRed = dmgRedUp * currentGrowth;
		nextLv.speed = spdUp * currentGrowth;
		return nextLv;
	}

	protected virtual void switchTarget() {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		lungeAttack.useItem ();
		targetchanged = false;
	}
}