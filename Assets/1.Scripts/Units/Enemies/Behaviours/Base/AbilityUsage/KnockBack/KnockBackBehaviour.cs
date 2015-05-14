using UnityEngine;
using System.Collections;

public class KnockBackBehaviour : EnemyBehaviour {

	protected SynthKnockBack knockback;

	public virtual void SetVar(SynthKnockBack knockback) {
		this.knockback = knockback;
	}
}
