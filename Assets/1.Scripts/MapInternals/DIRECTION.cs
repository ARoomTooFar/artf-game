using System;

/*
 * Enum for cardinal and ordinal directions
 * 
 * Directions opposite each other on a compass
 * are set to be negative values of each other to
 * make finding the opposite direction easier.
 * 
 */
public enum DIRECTION : int {
	NonDirectional = 0,
	North = 1,
	South = -(int)North,
	East = 2,
	West = -(int)East,
	NorthEast = 3,
	SouthWest = -(int)NorthEast,
	SouthEast = 4,
	NorthWest = -(int)SouthEast,
};

public enum DIRECTION2 : int {
	NonDirectional = -1,
	North = 0,
	NorthEast = 1,
	East = 2,
	SouthEast = 3,
	South = 4,
	SouthWest = 5,
	West = 6,
	NorthWest = 7,
};
