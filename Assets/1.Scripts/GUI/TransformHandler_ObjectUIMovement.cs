using UnityEngine;
using System.Collections;

//This class is for keeping the UI that is attached to an object in world
//oriented in the direction of the camera.
public class TransformHandler_ObjectUIMovement : MonoBehaviour 
{
	public Transform thing; //In-world object this little UI thing is sticking to
	public Camera cam; //Camera to make the UI face (must mimic its rotation)
	public Canvas objectUICanvas; //so we can access the Event Camera field in the Object UI Canvas

	void Start(){
		//must set these fields programmatically, because when instantiating a prefab,
		//these fields do not work if things are drag-and-dropped
		cam = GameObject.Find("UICamera").GetComponent<Camera>();
		objectUICanvas = this.gameObject.GetComponent("Canvas") as Canvas;
		objectUICanvas.worldCamera = cam; //required in order for buttons on canvas to react to clicks
	}

	void LateUpdate () 
	{
		//Set the UI's position to the object's position
		Vector3 p = new Vector3();
		p = thing.transform.position;
		transform.position = p; 

		//Set the UI's rotation to the camera's rotation.
		//This makes it so the UI is always facing the camera
		p = cam.transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler(p);
		

	}
}
