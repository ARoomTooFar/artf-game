using UnityEngine; 
using System.Collections;

public class DragRigidBodyCSharp : MonoBehaviour
{ 
	public float maxDistance = 100.0f;
	public float spring = 50.0f;
	public float damper = 5.0f;
	public float drag = 10.0f;
	public float angularDrag = 5.0f;
	public float distance = 0.2f;
	public bool attachToCenterOfMass = false;
	private SpringJoint springJoint;
	public TileMap tileMap;
	public GameObject draggedObject;
	public Camera cam;
	
	void Update ()
	{ 
		if(!Input.GetMouseButtonDown(0)) 
			return; 
		
//		if (Input.GetMouseButtonDown (0)) {
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit; 

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.gameObject.name != "Ground") {
					draggedObject = hit.collider.gameObject; 
					StartCoroutine (DragObject (hit.distance)); 
				}
			}
//		}


//		if(!springJoint) 
//		{ 
//			GameObject go = new GameObject("Rigidbody dragger"); 
//			Rigidbody body = go.AddComponent<Rigidbody>(); 
//			body.isKinematic = true; 
//			springJoint = go.AddComponent<SpringJoint>(); 
//		} 
		
//		springJoint.transform.position = hit.point; 
//		if(attachToCenterOfMass) 
//		{ 
//			Vector3 anchor = transform.TransformDirection(hit.rigidbody.centerOfMass) + hit.rigidbody.transform.position; 
//			anchor = springJoint.transform.InverseTransformPoint(anchor); 
//			springJoint.anchor = anchor; 
//		} 
//		else 
//		{ 
//			springJoint.anchor = Vector3.zero; 
//		} 
//		
//		springJoint.spring = spring; 
//		springJoint.damper = damper; 
//		springJoint.maxDistance = distance; 
//		springJoint.connectedBody = hit.rigidbody; 


	}
	
	IEnumerator DragObject (float distance)
	{ 
//		float oldDrag             = springJoint.connectedBody.drag; 
//		float oldAngularDrag     = springJoint.connectedBody.angularDrag; 
//		springJoint.connectedBody.drag             = this.drag; 
//		springJoint.connectedBody.angularDrag     = this.angularDrag; 
//		Camera cam = FindCamera(); 
		
		while (Input.GetMouseButton(0)) { 
//			Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
//			springJoint.transform.position = new Vector3(ray.GetPoint(distance).x, 1f, ray.GetPoint(distance).z); 
//			yield return null; 



			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			
			/* getting raycast info and logic */
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {


//				if(hitInfo.collider.gameObject.name == "TileMap"){
//					int x = Mathf.RoundToInt (hitInfo.point.x / tileMap.tileSize);
//					int z = Mathf.RoundToInt (hitInfo.point.z / tileMap.tileSize);
//					draggedObject.transform.position = new Vector3 (x * 1.0f, 1f, z * 1.0f);
//				}



				switch (hitInfo.collider.gameObject.name) {
				case "TileMap":
					int x = Mathf.RoundToInt (hitInfo.point.x / tileMap.tileSize);
					int z = Mathf.RoundToInt (hitInfo.point.z / tileMap.tileSize);
//					springJoint.transform.position = new Vector3(x * 1.0f, 0f, z*1.0f);

					draggedObject.transform.position = new Vector3 (x * 1.0f, 1f, z * 1.0f);

//					yield return null; 
					break;
////				if(hitInfo.collider.gameObject.name == "TileMap"){
////					
				}
			} else { 
//				springJoint.transform.position = new Vector3(ray.GetPoint(distance).x, 1f, ray.GetPoint(distance).z); 

			}
			yield return null; 
		}
		
//		if(springJoint.connectedBody) 
//		{ 
//			springJoint.connectedBody.drag             = oldDrag; 
//			springJoint.connectedBody.angularDrag     = oldAngularDrag; 
//			springJoint.connectedBody                 = null; 
//		} 
	}
	
	Camera FindCamera ()
	{ 
		if (camera) 
			return camera;
		else 
			return Camera.main; 
	} 
}