using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewFoliantFodder: NewMobileEnemy {
	
	protected Roll roll;
	
	public FoliantHive hive;
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
		stats.maxHealth = 25;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 5;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}

	public override void SetTierData(int tier) {
		tier = 0;
		base.SetTierData (tier);
		
		this.stats.speed = tier < 3 ? 9 : 12;
		
		if (tier > 0) {
			
			roll = this.inventory.items[inventory.selected].GetComponent<Roll>();
			if (roll == null) Debug.LogWarning ("FoliantFodder does not have lunge equipped");
			
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
/*	//------------------//
	// Public Functions //
	//------------------//
	
	public virtual void setHive(FoliantHive parent)
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
	
	//--------------------------------//*/
	
	
	
}