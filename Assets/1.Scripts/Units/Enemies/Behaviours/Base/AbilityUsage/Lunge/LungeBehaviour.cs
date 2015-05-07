// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class LungeBehaviour : EnemyBehaviour {

	protected MonsterLunge lunge;

	public virtual void SetVar(MonsterLunge lunge) {
		this.lunge = lunge;
		Debug.Log ("Set lunge");
	}
}