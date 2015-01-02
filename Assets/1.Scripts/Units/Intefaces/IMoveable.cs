// Movement Interface
// For units that can move

using UnityEngine;
using System.Collections;

public interface IMoveable {
	void moveCommands(); // Movement function, moves based on input
	void animationUpdate(); // Changes animations based on what unit is doing
	// void forceMove(); 
}
