using UnityEngine;
using System.Collections;

public class StickToThing : MonoBehaviour {
	public Transform thing;
	public Camera cam;



	public RectTransform rects;

	void Start () {
	
	}

	void Update () {
		Vector3 p = new Vector3();
		p = thing.transform.position;
		float f = p.x;

		
		
		//rects.transform.position = p;
		transform.position = p;




	}
}
