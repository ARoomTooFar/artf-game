// Stun Interface
// For units that can be stunned

using UnityEngine;
using System.Collections;

public interface IStunable {
	bool stun(); // Stuns unit causing them to be unable to act, return whether is was successful or not
	void removeStun();
}
