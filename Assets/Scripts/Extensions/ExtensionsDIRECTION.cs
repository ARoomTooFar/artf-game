using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ExtensionsDIRECTION
{
	/*
	 * Extension method for DIRECTION enum.
	 * 
	 * returns the direction opposite of the direction used to call this method.
	 * ex: DIRECTION.North.Opposite() == DIRECTION.South
	 * 
	 */
	public static DIRECTION Opposite(this DIRECTION dir){
		return (DIRECTION)(-(int)dir);
	}

	/*
	 * Extension method for DIRECTION enum.
	 * 
	 * Returns true if the direction is a cardinal direction (N S E W)
	 * Returns false otherwise
	 * 
	 */
	public static bool isCardinal(this DIRECTION dir){
		return Mathf.Abs((int)dir) == 1 || Mathf.Abs((int)dir) == 2;
	}

	/*
	 * Extension method for DIRECTION enum.
	 * 
	 * Returns true if the direction is an ordinal direction (NW NE SE SW)
	 * Returns false otherwise
	 * 
	 */
	public static bool isOrdinal(this DIRECTION dir){
		return Mathf.Abs((int)dir) == 3 || Mathf.Abs((int)dir) == 4;
	}
}


