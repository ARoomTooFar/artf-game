using System;
using UnityEngine;

public static class Global {
	public static readonly Vector3 nullVector3 = new Vector3(-1, -1, -1);
	public static readonly Vector3 initCameraRotation = new Vector3(60,-45,0);
	public static readonly Vector3 initCameraPosition = new Vector3(12.5f, 30f, -12.5f);
	public static readonly float mouseDeadZone = 10f;
	public static readonly int grid_x = 300;
	public static readonly int grid_z = 300;
	public static bool inLevelEditor = false;
	public static readonly Plane ground = new Plane(Vector3.up, Vector3.zero);

	public static readonly String defaultLevel = "Name:New Map\t3\nTerminal\n0,0,0,7,0,7 0,0,8,7,0,15\nRoom\nrooms:\nScenery\nLevelEditor/Other/PlayerStartingLocation:3,0,3,North \nLevelEditor/Other/PlayerEndingLocation:3,0,11,North \n{0}/Rooms/doortile:2,0,7,North 5,0,7,North 2,0,8,South 5,0,8,South \nMonster";

	public static float Normalize(float x, float min, float max){
		return (x - min) / (max - min);
	}
}