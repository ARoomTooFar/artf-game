using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfinderDebugDraw : MonoBehaviour {

	public Material lineMat;
	public Vector3 start;
	public Vector3 end;
	public Vector3 offset;

	// Update is called once per frame
	void Update () {

	}

	void OnPostRender(){
		List<Vector3> path = null;
		path = Pathfinder.getPath(start, end);
		drawPath(path);
		drawRoomPaths();

	}

	void drawPath (List<Vector3> path)
	{
		if(path == null) {
			return;
		}

		GL.Begin (GL.LINES);
		lineMat.SetPass (0);

		for (int i = 0; i < path.Count-1; ++i) {
			GL.Vertex (path[i] + offset);
			GL.Vertex (path[i+1] + offset);
		}
		GL.End ();
	}

	void drawRoomPaths(){
		foreach(ARTFRoom rm in MapData.TheFarRooms.roomList) {
			foreach(List<Vector3> path in rm.RoomPaths.Values){
				drawPath(path);
			}
		}
	}
}
