// Parent scripts for enemy units

using UnityEngine;
using System.Collections;

public class Enemy : Character, IStunable<float>, IForcible<float> {
	
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

	// Change this for other units in the future, ie. Unit that can be stunned and those that can't
	public virtual void stun(float stunDuration) {
		print ("Stunned for " + stunDuration + " seconds");
	}

	// The duration are essentially stun, expand on these later
	public virtual void pull(float pullDuration) {
		stun(pullDuration);
	}
	
	public virtual void push(float pushDuration) {
		stun(pushDuration);
	}


	/*//Use for other shit maybe
	public virtual void OnTriggerEnter(Collider other) {
		damage (1);
	}*/
}