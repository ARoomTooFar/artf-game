using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TestChar_RigidBody : MonoBehaviour {

	public float speed = 100.0f;
	public float rotateSpeed = 3.0f;
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up

	// Use this for initialization
	void Start () {
	}
	
	void FixedUpdate() {
		
	}
	
	// Update is called once per frame
	void Update () {
		moveCommands ();
	}
	
	/* Seperate these functions into an interface/superclass later */
	
	void moveCommands() {
		// Commands that only work on the ground go here
		if (isGrounded()) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			
			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			rigidbody.velocity = movement.normalized * speed;
		}
	}
	
	// Checks if user is grounded by sending a raycast to the ground
	// If called multiple times per update, consider making a bool variable and only call once per updaye
	public bool isGrounded () {
		return Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
	}
	
	/****************************************************************/
}
