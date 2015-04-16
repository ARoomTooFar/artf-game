using System;
using UnityEngine;

public static class Global {
	public static readonly Vector3 nullVector3 = new Vector3(-1, -1, -1);
	public static readonly Vector3 initCameraRotation = new Vector3(60,-30,0);
	public static readonly Vector3 initCameraPosition = new Vector3(12.5f, 30f, -12.5f);
	public static readonly float mouseDeadZone = 10f;
	public static readonly int grid_x = 300;
	public static readonly int grid_z = 300;
	public static bool inLevelEditor = false;
	
	public static UpgradeUI currentActiveObjectUI = null;
}

