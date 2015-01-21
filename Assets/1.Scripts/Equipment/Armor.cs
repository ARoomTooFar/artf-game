using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorStats {
	 public int armVal, goldVal, strength, coordination, luck, health, speed;
	 [Range(1,11)]
	 public int upgrade;
	 
}

public class Armor : Equipment {

	public ArmorStats stats;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}


}
