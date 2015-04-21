using UnityEngine;
using System.Collections;

public class MainMenuCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void MoveCameraDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        MoveCameraDown();
	}
}
