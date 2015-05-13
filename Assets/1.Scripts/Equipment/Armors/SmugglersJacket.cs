using UnityEngine;
using System.Collections;

[System.Serializable]
public class SmugglersJacketStats {
	 public int armVal, goldVal, strength, coordination, luck, health, speed;
	 [Range(0,10)]
	 public int upgrade;
	 public void worstCase(){
		armVal = 3;
		goldVal = 1;
		upgrade = 0;
		strength = 12;
		coordination = 14;
		luck = 0;
		health = 120;
		speed = 0;
	 }
}

public class SmugglersJacket : Equipment {

	public SmugglersJacketStats stats;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();
		stats.armVal = 1;
		stats.goldVal = 1;
		
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}


}
