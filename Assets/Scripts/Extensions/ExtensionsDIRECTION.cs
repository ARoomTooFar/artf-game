using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionsDIRECTION {
	/*
	 * public static DIRECTION Opposite(this DIRECTION dir)
	 * 
	 * Extension method for DIRECTION enum.
	 * 
	 * returns the direction opposite of the direction used to call this method.
	 * ex: DIRECTION.North.Opposite() == DIRECTION.South
	 * 
	 */
	public static DIRECTION Opposite(this DIRECTION dir) {
		return (DIRECTION)(-(int)dir);
	}

	/*
	 * public static bool isCardinal(this DIRECTION dir)
	 * 
	 * Extension method for DIRECTION enum.
	 * 
	 * Returns true if the direction is a cardinal direction (N S E W)
	 * Returns false otherwise
	 * 
	 */
	public static bool isCardinal(this DIRECTION dir) {
		return Mathf.Abs((int)dir) == 1 || Mathf.Abs((int)dir) == 2;
	}

	/*
	 * public static bool isOrdinal(this DIRECTION dir)
	 * 
	 * Extension method for DIRECTION enum.
	 * 
	 * Returns true if the direction is an ordinal direction (NW NE SE SW)
	 * Returns false otherwise
	 * 
	 */
	public static bool isOrdinal(this DIRECTION dir) {
		return Mathf.Abs((int)dir) == 3 || Mathf.Abs((int)dir) == 4;
	}

	/*
	 * public static DIRECTION QuarterTurn(this DIRECTION dir, bool goClockwise = true)
	 * 
	 * Extension method for DIRECTION enum.
	 * 
	 * Returns the direction 90 degrees to the input direction
	 * Direction to turn is determined by goClockwise.
	 * 
	 */
	public static DIRECTION QuarterTurn(this DIRECTION dir, bool goClockwise = true) {
		//if dir is non-directional, return non-directional
		if(dir == DIRECTION.NonDirectional) {
			return dir;
		}

		DIRECTION retVal = ((DIRECTION2)((int)dir.toDir2() + 2) % 8).toDir1();

		//if clockwise rotation, return, if counter clockwise, return the opposite direction
		return goClockwise ? retVal : retVal.Opposite();
	}

	/*
	 * public static Vector3 toRotationVector(this DIRECTION dir)
	 * 
	 * Extension method for DIRECTION enum.
	 * 
	 * Returns a Vector3 for use as a eulerAngles rotation
	 * 
	 */
	public static Vector3 toRotationVector(this DIRECTION dir) {
		float val = (int)(dir.toDir2());
		if(val < 0) {
			val = 0;
		}
		val *= 45;
		return new Vector3(0, Mathf.Round(val), 0);
	}

	public static DIRECTION getOrdinalFromCardinals(this DIRECTION dir1, DIRECTION dir2){
		if(!dir1.isCardinal() || !dir2.isCardinal()) {
			throw new UnityException("NonCardinal direction passed to getOrdinalFromCardinals");
		}
		if(dir1.Opposite().Equals(dir2)) {
			throw new UnityException("Directions are opposite");
		}
		string dir1str = dir1.ToString();
		string dir2str = dir2.ToString();

		string dirstr = null;

		if(dir1str.Length == 4) {
			dirstr = dir2str + dir1str;
		} else {
			dirstr = dir1str + dir2str;
		}

		return (DIRECTION)Enum.Parse(typeof(DIRECTION), dirstr);
	}

	public static DIRECTION2 toDir2(this DIRECTION dir){
		return Enum.Parse(DIRECTION2, dir.ToString()) as DIRECTION2;
	}

	public static DIRECTION toDir1(this DIRECTION2 dir){
		return Enum.Parse(DIRECTION, dir.ToString()) as DIRECTION;
	}
}


