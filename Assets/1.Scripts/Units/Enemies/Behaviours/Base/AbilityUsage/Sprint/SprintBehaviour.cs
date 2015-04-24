// For Behaviours that need a sprint reference

using UnityEngine;

public class SprintBehaviour : EnemyBehaviour {

	protected Sprint sprint;

	public virtual void SetVar(Sprint sprint) {
		this.sprint = sprint;
	}
}