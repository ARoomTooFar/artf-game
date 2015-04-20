using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfinderDebugDraw : MonoBehaviour {

	public Material lineMat;
	public Vector3 start;
	public Vector3 end;
	Vector3 prevStart;
	Vector3 prevEnd;
	public Vector3 offset;
	List<Vector3> path;
	public bool viewRoomPaths = true;
	public bool viewPath = true;

	// Update is called once per frame
	void Update () {
		if(prevStart != start || prevEnd != end) {
			prevStart = start;
			prevEnd = end;
			path = Pathfinder.getPath(start, end);
			if(path == null){
				Debug.Log("Invalid Start and/or End point");
			}
		}
	}

	void OnPostRender(){
		path = Pathfinder.getPath(start, end);
		if(viewPath) {
			drawPath(path);
		}
		if(viewRoomPaths) {
			drawRoomPaths();
		}

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
