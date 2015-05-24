using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoliantHive : StationaryEnemy {
	public int maxSpawn;
	public float spawnCD;
	public AggroTable hiveMindAggro;
	
	protected int currentSpawn;
	protected float nextUse;
	protected ShootFodderBall spawn;
	protected TargetCircle curTCircle;
	protected List<FoliantFodder> fodderList;
	
	protected override void Awake () {
		base.Awake ();
		fodderList = new List<FoliantFodder> ();
	}
	
	protected override void Start (){
		base.Start ();
		hiveMindAggro = base.aggroT;
		currentSpawn = 0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}
	
	protected override void Update() {
		base.Update ();
	}
	

	public override void SetTierData(int tier) {
		tier = 0;
		base.SetTierData (tier);
			
		spawn = this.inventory.items[inventory.selected].GetComponent<ShootFodderBall>();
		if (this.spawn == null) print("Foliant Hive has no spawner detected");
			
		foreach(SpawnBehaviour behaviour in this.animator.GetBehaviours<SpawnBehaviour>()) {
			behaviour.SetVar(this.spawn, this);
		}
		
		this.maxSpawn = tier < 3 ? tier + 2 : tier + 1;
	}



	
	//----------------------//
	
	//-------------------//
	
	//----------------//
	//Public Functions//
	//----------------//
	
	public virtual void addFodder(FoliantFodder newFodder){
		fodderList.Add (newFodder);
		currentSpawn = fodderList.Count;
	}
	
	public virtual void removeFodder(FoliantFodder deadFodder){
		fodderList.Remove (deadFodder);
		currentSpawn = fodderList.Count;
	}

	public virtual bool canSpawn(){
		return (currentSpawn < maxSpawn);
	}
	
	protected virtual void Spawn() {
		this.spawn.useItem();
	}
	
	//----------------//
	
	//--------------------------------//
	//Stationary Enemy Inherited Functions//
	//--------------------------------//
	
	public override void die() {
		
		foreach (FoliantFodder f in fodderList) {
			f.hiveDied();
		}

		deathNoise ();
		
		base.die ();
	}
	
	//--------------------------------//


}
