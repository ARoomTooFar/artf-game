using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Focuses camera on rooms players are in
 */
public class CameraAdjuster : MonoBehaviour {
	public CameraHitBox camHitBox;
	//GameObject[] playerList;
	public List<GameObject> playerList2 = new List<GameObject>();
	Dictionary<GameObject, Floor> roomMinMaxes = new Dictionary<GameObject, Floor>();
	HashSet<GameObject> roomsThatShouldBeInViewPort = new HashSet<GameObject>();
	List<GameObject> cameraBoundingPlanes = new List<GameObject>();
	Camera cam;
	
	float camStrafeSpeed = 6f;
	float camZoomSpeed = 20f;
	
	/*
	 * Holds information about an individual room that we need
	 * in order to adjust camera zoom level and position
	 */
	public class Floor{
		public float minX = 0;
		public float maxX = 0;
		public float minZ = 0;
		public float maxZ = 0;
		public Vector3[] corners;
		
		public Floor(float xMin, float xMax, float zMin, float zMax){
			minX = xMin;
			maxX = xMax;
			minZ = zMin;
			maxZ = zMax;
			
			corners = new Vector3[4];
			corners[0] = new Vector3(xMin, 0f, zMin);
			corners[1] = new Vector3(xMin, 0f, zMax);
			corners[2] = new Vector3(xMax, 0f, zMin);
			corners[3] = new Vector3(xMax, 0f, zMax);
		}
	}
	
	void Start () {
		transform.rotation = Quaternion.Euler(Global.initCameraRotation);
		transform.position = Global.initCameraPosition;
		
		//StartCoroutine(updateRoomsThatShouldBeInViewport());
	}
	
	public void InstantiatePlayers () {
		//playerList = new GameObject[4];
		//playerList[0] = GameObject.FindGameObjectWithTag ("Player1");
		//playerList[1] = GameObject.FindGameObjectWithTag ("Player2");
		//playerList[2] = GameObject.FindGameObjectWithTag ("Player3");
		//playerList[3] = GameObject.FindGameObjectWithTag ("Player4");
		if (GameObject.FindGameObjectWithTag ("Player1") != null)
			playerList2.Add (GameObject.FindGameObjectWithTag ("Player1"));
		if (GameObject.FindGameObjectWithTag ("Player2") != null)
			playerList2.Add (GameObject.FindGameObjectWithTag ("Player2"));
		
		//fill in dictionary that holds room corner locations
		GameObject[] roomFloorList = GameObject.FindGameObjectsWithTag("Floor");
		for(int i = 0; i < roomFloorList.Length; i++){
			roomMinMaxes.Add(roomFloorList[i], new Floor(
				roomFloorList[i].GetComponent<Renderer>().bounds.min.x,
				roomFloorList[i].GetComponent<Renderer>().bounds.max.x,
				roomFloorList[i].GetComponent<Renderer>().bounds.min.z,
				roomFloorList[i].GetComponent<Renderer>().bounds.max.z
				));
		}
		
		cam = this.gameObject.GetComponent<Camera>();
	}
	
	/*
	 * Renders planes with colliders around edges of camera frustum.
	 * This can be used to block players from leaving camera viewport.
	 */
	void renderNewFrustumPlanes(){
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
		int h = 0;
		while (h < planes.Length) {
			GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
			p.name = "Plane " + h.ToString();
			p.transform.position = -planes[h].normal * planes[h].distance;
			p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[h].normal);
			p.transform.localScale = new Vector3(50f, 50f, 50f);
			p.transform.SetParent(this.gameObject.transform);
			cameraBoundingPlanes.Add(p);
			h++;
		}
	}
	
	/*
	 * Finds all rooms that any player is currently in
	 */
	/*
	IEnumerator updateRoomsThatShouldBeInViewport(){
		while(true){
			if(playerList != null){
				HashSet<GameObject> roomsPlayersAreIn = new HashSet<GameObject>();


				for(int i = 0; i < playerList.Length; i++){
					if(playerList[i].GetComponent<Player>().isDead) continue;
					foreach(GameObject key in roomMinMaxes.Keys){
						if(playerList[i].transform.position.x < roomMinMaxes[key].maxX
						   && playerList[i].transform.position.x > roomMinMaxes[key].minX
						   && playerList[i].transform.position.z < roomMinMaxes[key].maxZ
						   && playerList[i].transform.position.z > roomMinMaxes[key].minZ
						   )
						{
							roomsPlayersAreIn.Add(key);
						}
					}
				}




				roomsThatShouldBeInViewPort = roomsPlayersAreIn;
			}
			yield return null;
		}
	}
	*/
	
	HashSet<GameObject> getRooms(){
		HashSet<GameObject> roomsPlayersAreIn = null;
		if(playerList2.Count != null){
			roomsPlayersAreIn = new HashSet<GameObject>();
			/*
			for(int i = 0; i < playerList.Length; i++){
				if(playerList[i].GetComponent<Player>().isDead) continue;
				foreach(GameObject key in roomMinMaxes.Keys){
					if(playerList[i].transform.position.x < roomMinMaxes[key].maxX
					   && playerList[i].transform.position.x > roomMinMaxes[key].minX
					   && playerList[i].transform.position.z < roomMinMaxes[key].maxZ
					   && playerList[i].transform.position.z > roomMinMaxes[key].minZ
					   )
					{
						roomsPlayersAreIn.Add(key);
					}
				}
			}
			*/
			
			for(int i = 0; i < playerList2.Count; i++){
				if(playerList2[i] == null || playerList2[i].GetComponent<Player>().isDead)
					playerList2.Remove(playerList2[i]);
				foreach(GameObject key in roomMinMaxes.Keys){
					if(playerList2[i].transform.position.x < roomMinMaxes[key].maxX
					   && playerList2[i].transform.position.x > roomMinMaxes[key].minX
					   && playerList2[i].transform.position.z < roomMinMaxes[key].maxZ
					   && playerList2[i].transform.position.z > roomMinMaxes[key].minZ
					   )
					{
						roomsPlayersAreIn.Add(key);
					}
				}
			}
		}
		return roomsPlayersAreIn;
	}
	
	void Update () {
		//if they're all dead, don't do anything
		if (this.playerList2 != null && this.playerList2.Count == 0) return;
		
		bool needToZoomOut = false;
		bool needToZoomIn = false;
		float mostOffCorner = 9999f;
		Vector3 roomAvgPosition = new Vector3(0f, 0f, 0f);
		
		roomsThatShouldBeInViewPort = getRooms();
		
		if(roomsThatShouldBeInViewPort != null && roomsThatShouldBeInViewPort.Count != 0){
			float mostOffCornerX = 9999f;
			float mostOffCornerY = 9999f;
			foreach(GameObject room in roomsThatShouldBeInViewPort){
				
				//for finding centroid of all rooms
				roomAvgPosition += room.transform.position;
				
				//find the corner of all rooms that is the farthest off screen
				for(int i = 0; i < roomMinMaxes[room].corners.Length; i++){
					Vector3 screenPos = cam.WorldToScreenPoint(roomMinMaxes[room].corners[i]);
					
					mostOffCornerX = Mathf.Min(mostOffCornerX, screenPos.x);
					mostOffCornerX = Mathf.Min(mostOffCornerX, Screen.width - screenPos.x);
					
					mostOffCornerY = Mathf.Min(mostOffCornerY, screenPos.y);
					mostOffCornerY = Mathf.Min(mostOffCornerY, Screen.height - screenPos.y);
					
					mostOffCorner = Mathf.Min(mostOffCornerX, mostOffCornerY);
				}
			}
			
			//take the average of all room positions and send camera there
			roomAvgPosition /= roomsThatShouldBeInViewPort.Count;
			roomAvgPosition.y = transform.parent.position.y;
			transform.parent.position = Vector3.Lerp(transform.parent.position, roomAvgPosition, Time.deltaTime * camStrafeSpeed);
		}
		
		//determine whether we need to zoom in or out.
		//the float value denotes the deadzone
		if(mostOffCorner > 10f)
			needToZoomIn = true;
		else if(mostOffCorner < -10f)
			needToZoomOut = true;
		
		//lerp camera's orthographic size up or down
		float orthoSize = this.gameObject.GetComponent<Camera>().orthographicSize;
		if(needToZoomOut){
			this.gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(orthoSize, orthoSize + 0.5f, Time.deltaTime * camZoomSpeed);
		}else if(needToZoomIn){
			this.gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(orthoSize, orthoSize - 0.5f, Time.deltaTime * camZoomSpeed);
		}
	}
	
}
