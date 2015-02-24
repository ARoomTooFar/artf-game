// Force Interface
// For units that can be forcibly moves
// Not for teh mechanics of the force but what the character does while forced and also for checking if the unit can be pulled
// Requires Stun interface to also be implemented and possibly movement interface if we don't want stationary units to be moved out of place
// We can reimitate the stun system in this code if required (Although that is just terrible)

using UnityEngine;
using System.Collections;

public interface IForcible<D, S> {
	bool knockback(D direction, S speed);
	void stabled();
	void pull(S pullDuration); // pull (eg Hookshot)
	void push(S pushDuration); // push (eg something pushy)
}
