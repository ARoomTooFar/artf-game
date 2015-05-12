using UnityEngine;
using System.Collections;

[System.Serializable]
public class DelversDusterStats {
	 public int armVal, goldVal, strength, coordination, luck, health, speed;
	 [Range(0,10)]
	 public int upgrade;
	 public void worstCase(){
		armVal = 8;
		goldVal = 1;
		upgrade = 0;
		strength = 18;
		coordination = 35;
		luck = 0;
		health = 220;
		speed = 0;
	 }
}

public class DelversDuster : Equipment {

	public DelversDusterStats stats;

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
