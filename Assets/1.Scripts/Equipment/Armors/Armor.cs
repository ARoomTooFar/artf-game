using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorStats {
	public int armVal, goldVal, strength, coordination, health;
	
	//doThis
	public int ArmVal{
		get{return armVal * upgrade;}
	}
	public int upgrade;

	public ArmorStats(int armVal, int goldVal, int strength, int coordination, int health, int upgrade) {
		this.armVal = armVal;
		this.goldVal = goldVal;
		this.strength = strength;
		this.coordination = coordination;
		this.health = health;
	}

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
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(1, 1, 0, 0, 0, tier);
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}


}
