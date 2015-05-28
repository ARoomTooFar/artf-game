using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraAdjuster : MonoBehaviour {
	public CameraHitBox camHitBox;
	Vector3 diffSpot;
	GameObject[] playerList;
	float minAllowedOrthoSize = 13f;
	Dictionary<GameObject, Floor> roomMinMaxes = new Dictionary<GameObject, Floor>();
	HashSet<GameObject> roomsThatShouldBeInViewPort = new HashSet<GameObject>();
	List<GameObject> cameraBoundingPlanes = new List<GameObject>();
	Camera cam;

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
		diffSpot = transform.position - camHitBox.transform.position;

		StartCoroutine(updateRoomsThatShouldBeInViewport());
		StartCoroutine(adjustCameraToEncapsulateRoomsPlayersAreIn());
	}
	
	public void InstantiatePlayers () {
		playerList = new GameObject[4];
		playerList[0] = GameObject.FindGameObjectWithTag ("Player1");
		playerList[1] = GameObject.FindGameObjectWithTag ("Player2");
		playerList[2] = GameObject.FindGameObjectWithTag ("Player3");
		playerList[3] = GameObject.FindGameObjectWithTag ("Player4");

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
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

//		int h = 0;
//		while (h < planes.Length) {
//			GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
//			p.name = "Plane " + h.ToString();
//			p.transform.position = -planes[h].normal * planes[h].distance;
//			p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[h].normal);
//			p.transform.localScale = new Vector3(50f, 50f, 50f);
//			p.transform.SetParent(this.gameObject.transform);
//			cameraBoundingPlanes.Add(p);
//			h++;
//		}
		
	}
	
	bool isAPlayerOffScreen(){
		for(int i = 0; i < playerList.Length; i++){
			if(playerList[i] == null) continue;
			if(!playerList[i].GetComponent<Renderer>().isVisible)
				return true;
		}
		return false;
	}

	IEnumerator adjustCameraToEncapsulateRoomsPlayersAreIn(){
		while(true){
			if(roomsThatShouldBeInViewPort != null){
//				print(roomsThatShouldBeInViewPort.Count);
//				foreach(GameObject room in roomsThatShouldBeInViewPort){
//					print ("checking");
//					while(!room.GetComponent<Renderer>().isVisible){
//						print("not visible");
//						float orthoSize = this.gameObject.GetComponent<Camera>().orthographicSize;
//						this.gameObject.GetComponent<Camera>().orthographicSize = 
//							Mathf.Lerp(orthoSize, orthoSize + 1f, Time.deltaTime * 4f);
//						yield return null;
//					}
//				}
			}
			yield return null;
		}
	}

	IEnumerator updateRoomsThatShouldBeInViewport(){
		while(true){
			if(playerList != null){
				HashSet<GameObject> roomsPlayersAreIn = new HashSet<GameObject>();

				for(int i = 0; i < playerList.Length; i++){
					foreach(GameObject key in roomMinMaxes.Keys){
						if(playerList[i].transform.position.x < roomMinMaxes[key].maxX
						   && playerList[i].transform.position.x > roomMinMaxes[key].minX
						   && playerList[i].transform.position.z < roomMinMaxes[key].maxZ
						   && playerList[i].transform.position.z > roomMinMaxes[key].minZ
						   )
						{
							roomsPlayersAreIn.Add(key);
						}


						
//						if(key.GetComponent<Renderer>().isVisible) print (key.GetInstanceID() + " is visible");
					}
				}
				roomsThatShouldBeInViewPort = roomsPlayersAreIn;
			}
			yield return null;
		}

	}

	void Update () {

		if (this.playerList != null && this.playerList.Length == 0) return;



		float minDistanceFromEdge = 0f;

//		Vector3 centroid = new Vector3();

		bool needToZoomOut = false;
//		float maxX = -9999f;
//		float minX = 9999f;
//		float maxZ = -9999f;
//		float minZ = 9999f;
		List<GameObject> rooms = new List<GameObject>();
		Vector3 roomAvgPosition = new Vector3(0f, 0f, 0f);
		foreach(GameObject room in roomsThatShouldBeInViewPort){
			roomAvgPosition += room.transform.position;
			for(int i = 0; i < roomMinMaxes[room].corners.Length; i++){
//				if(roomMinMaxes[room].maxX > maxX)
//					maxX = roomMinMaxes[room].maxX;
//				if(roomMinMaxes[room].minX < minX)
//					minX = roomMinMaxes[room].minX;
//				if(roomMinMaxes[room].maxZ > maxZ)
//					maxZ = roomMinMaxes[room].maxZ;
//				if(roomMinMaxes[room].minZ < minZ)
//					minZ = roomMinMaxes[room].minZ;

				Vector3 screenPos = cam.WorldToScreenPoint(roomMinMaxes[room].corners[i]);

				if(screenPos.x < minDistanceFromEdge){
					needToZoomOut = true;
					break;
				}else if(Screen.width - screenPos.x < minDistanceFromEdge){
					needToZoomOut = true;
					break;
				}else if(Screen.height - screenPos.y < minDistanceFromEdge){
					needToZoomOut = true;
					break;
				}else if(screenPos.y < minDistanceFromEdge){
					needToZoomOut = true;
					break;
				}
			}
		}

		roomAvgPosition /= roomsThatShouldBeInViewPort.Count;

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		Plane p = new Plane(Vector3.up, Vector3.zero); 
		float rayDistance;
		if (p.Raycast(ray, out rayDistance)){
			print(ray.GetPoint(rayDistance));
		}
		
//		Vector3 centroid = new Vector3(maxX - minX, 0f, maxZ - minZ);
//		Vector3 nextStep = Vector3.Lerp(ray.GetPoint(rayDistance), roomAvgPosition, Time.deltaTime * 4f);
//		nextStep.y = transform.position.y;

//		GameObject g = GameObject.Instantiate(Resources.Load("Tombstone_RIP_obj"), roomAvgPosition, Quaternion.identity) as GameObject;
//		Vector3 nextStep = new Vector3(0f, transform.position.y, 0f);
//		if(Mathf.Abs(roomAvgPosition.x - ray.GetPoint(rayDistance).x) > 20f || Mathf.Abs(roomAvgPosition.z - ray.GetPoint(rayDistance).z) > 20f){
//			if(ray.GetPoint(rayDistance).x < roomAvgPosition.x){
//				nextStep.x = Mathf.Lerp(transform.position.x, transform.position.x + 1, Time.deltaTime * 4f);
//			}
//			if(ray.GetPoint(rayDistance).x > roomAvgPosition.x){
//				nextStep.x = Mathf.Lerp(transform.position.x, transform.position.x - 1, Time.deltaTime * 4f);
//			}
//			if(ray.GetPoint(rayDistance).z < roomAvgPosition.z){
//				nextStep.z = Mathf.Lerp(transform.position.z, transform.position.z + 1, Time.deltaTime * 4f);
//			}
//			if(ray.GetPoint(rayDistance).z > roomAvgPosition.z){
//				nextStep.z = Mathf.Lerp(transform.position.z, transform.position.z - 1, Time.deltaTime * 4f);
//			}
//		}

		roomAvgPosition.y = transform.parent.position.y;
//		roomAvgPosition.x -= 20f;
//		roomAvgPosition.z -= 20f;
		transform.parent.position = roomAvgPosition;



		
		
		
		//		for(int i = 0; i < playerList.Length; i++){
//			if(playerList[i] == null) continue;
//
//			Vector3 screenPos = cam.WorldToScreenPoint(playerList[i].transform.position);
//
//			if(screenPos.x < minDistanceFromEdge){
//				needToZoomOut = true;
//				break;
//			}else if(Screen.width - screenPos.x < minDistanceFromEdge){
//				needToZoomOut = true;
//				break;
//			}else if(Screen.height - screenPos.y < minDistanceFromEdge){
//				needToZoomOut = true;
//				break;
//			}else if(screenPos.y < minDistanceFromEdge){
//				needToZoomOut = true;
//				break;
//			}
//
//				
//		}

		float orthoSize = this.gameObject.GetComponent<Camera>().orthographicSize;
		if(needToZoomOut){
			this.gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(orthoSize, orthoSize + 1f, Time.deltaTime * 4f);
		}else{
//			this.gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(orthoSize, orthoSize - 1f, Time.deltaTime * 4f);
		}

//		transform.position = camHitBox.transform.position + diffSpot;
//		diffSpot = transform.position - camHitBox.transform.position;

	}

}
