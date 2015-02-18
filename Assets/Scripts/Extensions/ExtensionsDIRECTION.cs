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

	public static DIRECTION QuarterTurn(this DIRECTION dir, bool goClockwise = true){
		DIRECTION val;

		switch(dir) {
		case DIRECTION.North:
			val = DIRECTION.East;
			break;
		case DIRECTION.East:
			val = DIRECTION.South;
			break;
		case DIRECTION.South:
			val = DIRECTION.West;
			break;
		case DIRECTION.West:
			val = DIRECTION.North;
			break;
		case DIRECTION.NorthEast:
			val = DIRECTION.SouthEast;
			break;
		case DIRECTION.SouthEast:
			val = DIRECTION.SouthWest;
			break;
		case DIRECTION.SouthWest:
			val = DIRECTION.NorthWest;
			break;
		case DIRECTION.NorthWest:
			val = DIRECTION.NorthEast;
			break;
		default:
			val = DIRECTION.NonDirectional;
			break;
		}

		return goClockwise ? val : val.Opposite();
	}

	public static Vector3 toRotationVector(this DIRECTION dir){
		return new Vector3();
	}
}


