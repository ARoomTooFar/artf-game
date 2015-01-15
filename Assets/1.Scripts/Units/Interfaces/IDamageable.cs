// Damage Interface
// For units that can be hit and have health

using UnityEngine;
using System.Collections;

public interface IDamageable<D> {
	void damage(D dmgTaken); // Damage function, when damage is taken
}
