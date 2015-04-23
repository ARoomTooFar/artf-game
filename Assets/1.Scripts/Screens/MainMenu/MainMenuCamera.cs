using UnityEngine;
using System.Collections;

public class MainMenuCamera : MonoBehaviour {
    public bool slideDown = false;

	// Use this for initialization
	void Start () {
	
	}

    void MoveCameraDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (slideDown) {
            MoveCameraDown();
        }
	}
}
