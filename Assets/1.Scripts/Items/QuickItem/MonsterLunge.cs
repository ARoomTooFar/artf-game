// BushmanLunge item
//     If their target is in in the collider, the Bushman will lunge to them

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterLunge : Lunge {

	protected Enemy bushy;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	protected override void Update() {
		base.Update ();
	}

	public virtual void SetUp() {
		this.bushy = this.user.GetComponent<Enemy>();
		InvokeRepeating("CheckEnemies", 0f, 0.1f);
	}

	protected virtual void CheckEnemies() {
		if (this.bushy.target == null) return;
		if (this.enemiesInRange.Contains(this.bushy.target.GetComponent<Character>())) bushy.animator.SetBool ("LungeTargetIn", true);
		else bushy.animator.SetBool ("LungeTargetIn", false);
	}

	public override void useItem() {
		// base.useItem ();
		curCoolDown = cooldown;
		user.animationLock = true;
		this.StartCoroutine(LungeFunction(bushy.target.GetComponent<Character>()));
	}
}