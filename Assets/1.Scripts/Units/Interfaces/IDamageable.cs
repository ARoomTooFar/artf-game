// Damage Interface
// For units that can be hit and have health

using UnityEngine;
using System.Collections;

public interface IDamageable<D, P> {
	void damage(D dmgTaken, P atkPosition); // When damage taken depends on direction the unit is facing
	void damage(D dmgTaken); // When damage taken is independent of unit facing (ie, flame trap)
	void die(); // Called when killed
	void rez();
	void heal(D healTaken);
}
