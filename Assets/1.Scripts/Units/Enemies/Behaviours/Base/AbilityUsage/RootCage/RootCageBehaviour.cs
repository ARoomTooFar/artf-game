// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class RootCageBehaviour : EnemyBehaviour {
	
	protected RootCage cage;
	
	public virtual void SetVar(RootCage cage) {
		this.cage = cage;
	}
}