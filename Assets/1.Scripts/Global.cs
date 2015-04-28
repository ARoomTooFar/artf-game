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

	public static readonly String defaultLevel = "MapData\nTerrain\nTerminal\n-1,0,-8,6,0,-1 0,0,8,7,0,15\nRoom\nrooms:-1,0,-8,6,0,-1 0,0,8,7,0,15\nScenery\nLevelEditor/Other/PlayerStartingLocation:2,0,-5,North \nLevelEditor/Other \t/PlayerEndingLocation:3,0,11,North \nMonster";
}

