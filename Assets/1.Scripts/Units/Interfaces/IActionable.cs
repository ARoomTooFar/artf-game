// Action Interface
// For units actions and animation

using UnityEngine;
using System.Collections;

public interface IActionable<T> {
	void setActable(T actable);
	void actionCommands(); // The actions this unit can perform (Button input can go here or calling other functions for button input)
	void animationUpdate(); // Changes animations based on what unit is doing
}
