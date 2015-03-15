//Playing around with a healing interface to use.
//Handles resurrection
//Handles healing
using UnityEngine;
using System.Collections;

public interface IHealable<D> {
	void rez();
	void heal(D healTaken);
}
