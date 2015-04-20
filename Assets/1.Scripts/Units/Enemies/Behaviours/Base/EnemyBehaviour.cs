// All enemy AI behaviours should inherit from this instead of StateMachineBehaviour
//     This is for setting reference data

using UnityEngine;

public class EnemyBehaviour : StateMachineBehaviour {
	public NewEnemy unit;

	public virtual void SetVar(NewEnemy unit) {
		this.unit = unit;
	}
}