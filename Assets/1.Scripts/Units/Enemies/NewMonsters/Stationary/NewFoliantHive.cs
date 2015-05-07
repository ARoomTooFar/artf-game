using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFoliantHive : NewStationaryEnemy {
	public int maxSpawn;
	public float spawnCD;
	public AggroTable hiveMindAggro;
	
	protected int currentSpawn;
	protected float nextUse;
	protected ShootFodderBall spawn;
	protected TargetCircle curTCircle;
	protected List<NewFoliantFodder> fodderList;
	
	protected override void Awake () {
		base.Awake ();
		fodderList = new List<NewFoliantFodder> ();
	}
	
	protected override void Start (){
		base.Start ();
		hiveMindAggro = base.aggroT;
		currentSpawn = 0;
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
			
		spawn = this.inventory.items[inventory.selected].GetComponent<ShootFodderBall>();
		if (this.spawn == null) print("Foliant Hive has no spawner detected");
			
		foreach(SpawnBehaviour behaviour in this.animator.GetBehaviours<SpawnBehaviour>()) {
			behaviour.SetVar(this.spawn, this);
		}
	}



	
	//----------------------//
	
	//-------------------//
	
	//----------------//
	//Public Functions//
	//----------------//
	
	public virtual void addFodder(NewFoliantFodder newFodder){
		fodderList.Add (newFodder);
		currentSpawn = fodderList.Count;
	}
	
	public virtual void removeFodder(NewFoliantFodder deadFodder){
		fodderList.Remove (deadFodder);
		currentSpawn = fodderList.Count;
	}

	public virtual bool canSpawn(){
		return (currentSpawn < maxSpawn);
	}
	
	//----------------//
	
	//--------------------------------//
	//Stationary Enemy Inherited Functions//
	//--------------------------------//
	
	public override void die() {
		
		foreach (NewFoliantFodder f in fodderList) {
			f.hiveDied();
		}
		
		base.die ();
	}
	
	//--------------------------------//


}
