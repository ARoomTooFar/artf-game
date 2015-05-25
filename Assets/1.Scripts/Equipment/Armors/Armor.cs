using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorStats {
	public ArmorStats(int armVal, int goldVal, int strength, int coordination, int health, int upgrade) {
		this.armVal = armVal;
		this.goldVal = goldVal;
		this.strength = strength;
		this.coordination = coordination;
		this.health = health;
		this.upgrade = upgrade;
	}

	public int armVal, goldVal, strength, coordination, health, upgrade;
	//
	/////Armor Access Functions
	//

	//returns base armor value of gear
	public int ArmVal{
		get{return armVal;}
	}
	//returns base health value of gear
	public int Health{
		get{return health;}
	}
	//returns base strenght value of gear
	public int Strength{
		get{return strength;}
	}
	//returns base coordination value of gear
	public int Coordination{
		get{return coordination;}
	}
	//returns base gold value of gear
	public int GoldVal{
		get{return goldVal;}
	}

	//returns upgrade value of gear
	public int Upgrade{
		get{return upgrade;}
	}

	//returns armor value after upgrades
	public int ArmValUpgrade{
		get{
			int temp = armVal;
			//for every level upgraded the armor value is added to itself.
			for(int i = 0; i < upgrade; i++)
			{
				temp += temp;
			}
			return temp;
		}
	}
	//returns health value after upgrades
	public int HealthUpgrade{
		get{
			int temp = health;
			//for every level upgraded the health value is added to itself.
			for(int i = 0; i < upgrade; i++)
			{
				temp += temp;
			}
			return temp;}
	}
	//returns the coordination value after upgrades
	public int CoordinationUpgrade{
		get{
			int temp = coordination;
			//for every level upgraded the armor value is added to itself.
			for(int i = 0; i < upgrade; i++)
			{
				temp += temp;
			}
			return temp;}
	}
	//returns the gold value after upgrades
	public int GoldValUpgrade{
		get{return goldVal * (upgrade+5);}
	}
	//Access like this
	//ArmorStats.ArmVal;
	
	//[Range(0,10)]
	//public int upgrade;

	public void worstCase() {
		armVal = 1;
		goldVal = 1;
		strength = 0;
		coordination = 0;
		health = 0;
		upgrade = 0;
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
		stats = new ArmorStats(1, 1, 0, 0, 0, 0);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}


}
