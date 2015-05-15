// All enemy AI behaviours should inherit from this instead of StateMachineBehaviour
//     This is for setting reference data

using UnityEngine;

public class EnemyBehaviour : StateMachineBehaviour {
	protected Enemy unit;

	public virtual void SetVar(Enemy unit) {
		this.unit = unit;
	}
}