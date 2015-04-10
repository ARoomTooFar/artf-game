using System;
using UnityEngine;

public static class Global {
	public static readonly Vector3 nullVector3 = new Vector3(-1, -1, -1);
	public static readonly Vector3 initCameraRotation = new Vector3(45, -45, 0);
	public static readonly Vector3 initCameraPosition = new Vector3(43f, 15f, 2.5f);
	public static readonly float mouseDeadZone = 10f;
	public static readonly int grid_x = 300;
	public static readonly int grid_z = 300;
}

