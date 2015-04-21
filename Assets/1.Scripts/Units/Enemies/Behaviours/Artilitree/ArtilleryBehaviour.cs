// Artilitree parent behaviour for behaviours that need a reference to the weapon
using UnityEngine;

public class ArtilleryBehaviour : EnemyBehaviour {
	protected Artillery artillery;
	protected TargetCircle curTCircle;
	
	public virtual void SetVar(Artillery artillery) {
		this.artillery = artillery;
	}
}