// Slow Interface
// For units whose speed can be manipulated (File title a bit misleading)

using UnityEngine;
using System.Collections;

public interface ISlowable<F> {
	void slow(F slowStrength);
	void removeSlow(F slowStrength);
	void slowForDuration(F slowStrength, F slowDuration);

	void speed(F speedStrength);
	void removeSpeed(F speedStrength);
	void speedForDuration(F speedStrength, F slowDuration);
}
