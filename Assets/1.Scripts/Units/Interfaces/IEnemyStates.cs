using UnityEngine;
using System.Collections;

public interface IEnemyStates{

	bool Approaching();
	bool Attacking();
	bool Searching();
	bool Retreating();
	bool Resting();

}
