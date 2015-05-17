// RootRing ability used by artilitree to protect itself and trap enemy units

using UnityEngine;
using System.Collections;

public class RootCage : QuickItem {
	
	public GameObject roots;
	
	public Enemy eUser;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
		cooldown = 15.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void useItem() {
		base.useItem ();
		
		if (this.eUser.target == null) return;
		
		Vector3 curPos = this.eUser.target.transform.position;
		
		Instantiate(this.roots, new Vector3(curPos.x - 2.5f, 0.5f, curPos.z), Quaternion.identity);
		Instantiate(this.roots, new Vector3(curPos.x + 2.5f, 0.5f, curPos.z), Quaternion.identity);
		Instantiate(this.roots, new Vector3(curPos.x, 0.5f, curPos.z + 2.5f), Quaternion.identity);
		Instantiate(this.roots, new Vector3(curPos.x, 0.5f, curPos.z - 2.5f), Quaternion.identity);
		
		Instantiate(this.roots, new Vector3(curPos.x - 2.5f, 0.5f, curPos.z - 2.5f), Quaternion.identity);
		Instantiate(this.roots, new Vector3(curPos.x + 2.5f, 0.5f, curPos.z + 2.5f), Quaternion.identity);
		Instantiate(this.roots, new Vector3(curPos.x - 2.5f, 0.5f, curPos.z + 2.5f), Quaternion.identity);
		Instantiate(this.roots, new Vector3(curPos.x + 2.5f, 0.5f, curPos.z - 2.5f), Quaternion.identity);
		
		animDone();
	}
	
	protected override void animDone() {
		base.animDone();
	}
	
}
