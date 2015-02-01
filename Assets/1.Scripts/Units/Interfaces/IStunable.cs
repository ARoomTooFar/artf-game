// Stun Interface
// For units that can be stunned
// In the future, it may also be for units that can't be stunned and counter with something

using UnityEngine;
using System.Collections;

public interface IStunable<D> {
	void stun(D stunDuration); // Falling code goes here
}
