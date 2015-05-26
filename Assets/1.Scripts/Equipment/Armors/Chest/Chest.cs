using UnityEngine;
using System.Collections;

public class Chest : Armor {

	public SkinnedMeshRenderer keyArmor;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		if (this.keyArmor != null) this.GetComponent<SkinnedMeshRenderer>().sharedMesh = this.keyArmor.sharedMesh; // When we get more armor swap this over to a init stat type thing
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
}
