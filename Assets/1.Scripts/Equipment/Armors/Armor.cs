using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorStats {
	public ArmorStats(int armVal, int goldVal, int strength, int coordination, int health) {
		this.armVal = armVal;
		this.goldVal = goldVal;
		this.strength = strength;
		this.coordination = coordination;
		this.health = health;
	}

	public int armVal, goldVal, strength, coordination, health;

	//doThis
	public int ArmVal{
		get{return armVal * upgrade;}
	}
	//instead of this
	/*
	public int getArmVal(){
		return armVal * upgrade;
	}
	*/
	//Access like this
	//ArmorStats.ArmVal;



	[Range(0,10)]
	public int upgrade;

	public void worstCase() {
		armVal = 1;
		goldVal = 1;
		upgrade = 0;
		strength = 0;
		coordination = 0;
		health = 0;
	}
}

public class Armor : Equipment {

	public ArmorStats stats;

	// Use this for initialization
	protected override void Start() {
		base.Start();
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();
		stats = new ArmorStats(1, 1, 0, 0, 0);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}


}
