﻿// RootRing ability used by artilitree to protect itself and trap enemy units

using UnityEngine;
using System.Collections;

public class RootRing : QuickItem {

	public GameObject roots;

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

		Vector3 curPos = this.user.transform.position;

		SpawnTree(new Vector3(curPos.x - 3f, 1.5f, curPos.z));
		SpawnTree(new Vector3(curPos.x + 3f, 1.5f, curPos.z));
		SpawnTree(new Vector3(curPos.x, 1.5f, curPos.z + 3f));
		SpawnTree(new Vector3(curPos.x, 1.5f, curPos.z - 3f));
		
		SpawnTree(new Vector3(curPos.x - 3f, 1.5f, curPos.z - 3f));
		SpawnTree(new Vector3(curPos.x + 3f, 1.5f, curPos.z + 3f));
		SpawnTree(new Vector3(curPos.x - 3f, 1.5f, curPos.z + 3f));
		SpawnTree(new Vector3(curPos.x + 3f, 1.5f, curPos.z - 3f));

		animDone();
	}
	
	private void SpawnTree(Vector3 pos) {
		Instantiate(this.roots, pos, Quaternion.Euler(-90, 0, 0));
	}

	protected override void animDone() {
		base.animDone();
	}
	
}
