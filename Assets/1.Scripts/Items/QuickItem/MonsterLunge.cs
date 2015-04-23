// BushmanLunge item
//     If their target is in in the collider, the Bushman will lunge to them

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterLunge : Lunge {

	protected NewEnemy bushy;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	protected override void Update() {
		base.Update ();
	}

	public virtual void SetUp() {
		this.bushy = this.user.GetComponent<NewEnemy>();
		InvokeRepeating("CheckEnemies", 0f, 0.1f);
	}

	protected virtual void CheckEnemies() {
		if (this.bushy.target == null) return;
		if (this.enemiesInRange.Contains(this.bushy.target.GetComponent<Character>())) bushy.animator.SetBool ("LungeTargetIn", true);
		else bushy.animator.SetBool ("LungeTargetIn", false);
	}

	public override void useItem() {
		base.useItem ();
		user.stunned = true;
		this.StartCoroutine(LungeFunction(bushy.target.GetComponent<Character>()));
	}
}