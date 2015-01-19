// Parent scripts for enemy units

using UnityEngine;
using System.Collections;

public class Enemy : Character{
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void damage(int dmgTaken) {
		base.damage(dmgTaken);
		print ("Fuck: " + dmgTaken + " Damage taken");
	}

	public override void stun(float duration) {
		print ("Stunned for " + duration + " seconds");
	}

	/*//Use for other shit maybe
	public virtual void OnTriggerEnter(Collider other) {
		damage (1);
	}*/
}