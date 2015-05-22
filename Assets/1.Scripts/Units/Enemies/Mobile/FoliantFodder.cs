using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FoliantFodder: MobileEnemy {
	
	protected MonsterLunge lunge;
	
	public FoliantHive hive;
	public bool hiveMind;
	
	protected override void Awake () {
		hiveMind = false;
		base.Awake ();
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 4.0f;
	}
	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();
	}

	public override void SetTierData(int tier) {
		tier = 0;
		base.SetTierData (tier);

		monsterLoot.initializeLoot("FoliantFodder", tier);
		
		// this.stats.speed = tier < 3 ? 9 : 12;
		
		if (tier > 0) {
			
			lunge = this.inventory.items[inventory.selected].GetComponent<MonsterLunge>();
			if (lunge == null) Debug.LogWarning ("FoliantFodder does not have lunge equipped");

			foreach(LungeBehaviour behaviour in this.animator.GetBehaviours<LungeBehaviour>()) {
				behaviour.SetVar(this.lunge);
			}
			
			lunge.SetUp();
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
		deathNoise ();
		
		base.die ();
	}
	
	//--------------------------------//
	
	
	
}