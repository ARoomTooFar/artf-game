using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewFoliantFodder: NewMobileEnemy {
	
	protected MonsterLunge lunge;
	
	public NewFoliantHive hive;
	public bool hiveMind;
	
	protected override void Awake () {
		hiveMind = false;
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 50;
		stats.health = stats.maxHealth;
		stats.armor = 1;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 4.0f;
	}

	public override void SetTierData(int tier) {
		tier = 5;
		base.SetTierData (tier);

		monsterLoot.initializeLoot("FoliantFodder", tier);
		
		this.stats.speed = tier < 3 ? 9 : 12;
		
		if (tier > 0) {
			
			lunge = this.inventory.items[inventory.selected].GetComponent<MonsterLunge>();
			if (lunge == null) Debug.LogWarning ("FoliantFodder does not have lunge equipped");

			foreach(LungeBehaviour behaviour in this.animator.GetBehaviours<LungeBehaviour>()) {
				behaviour.SetVar(this.lunge);
			}
			
			/*foreach(Roll behaviour in this.animator.GetBehaviours<Roll>()) {
				behaviour.roll = this.roll;
			}
			
			foreach(FodderApproach behaviour in this.animator.GetBehaviours<FodderApproach>()) {
				behaviour.charge = this.charge;
			}
			*/
		}
	}
	
	//----------------------//
	// Transition Functions //
	//----------------------//


	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//

	
	//-------------------//


//	Add this back in when everything else is done	
	//------------------//
	// Public Functions //
	//------------------//
	
	public virtual void setHive(NewFoliantHive parent)
	{
		hiveMind = true;
		hive = parent;
		base.aggroT = hive.hiveMindAggro;
		
	}
	
	public virtual void hiveDied()
	{
		hiveMind = false;
		hive = null;
		base.aggroT = new AggroTable ();
	}
	
	//------------------//
	
	//--------------------------------//
	//Mobile Enemy Inherited Functions//
	//--------------------------------//
	
	public override void die() {
		
		if (hiveMind) {
			hive.removeFodder(this);
		}
		
		base.die ();
	}
	
	//--------------------------------//
	
	
	
}