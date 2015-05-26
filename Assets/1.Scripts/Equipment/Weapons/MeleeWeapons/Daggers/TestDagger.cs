using UnityEngine;
using System.Collections;

public class TestDagger : Dagger {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		//base.setInitValues();
		
		//stats.damage = 4;
		base.setInitValues();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
}
