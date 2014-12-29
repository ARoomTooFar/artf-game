using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TestChar_RigidBody : MonoBehaviour {

	public float speed = 5.0f;
	public float gravity = 50.0f;
	public float rotateSpeed = 3.0f;
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		facing = Vector3.forward;
	}
	
	void FixedUpdate() {
		moveCommands ();
	}
	
	// Update is called once per frame
	void Update () {
		animationUpdate();
	}
	
	/* Seperate these functions into an interface/superclass later */
	
	private void moveCommands() {
		// Commands that only work on the ground go here
		if (isGrounded()) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			
			facing = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			
			// Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			
			rigidbody.velocity = facing.normalized * speed;
		} else {
			// fake gravity
			// Animation make it so rigidbody gravity works oddly due to some gravity weight
			// Seems like Unity Pro is needed to change that, so unless we get it, this will suffice 
			rigidbody.velocity = new Vector3 (0.0f, -gravity, 0.0f);
		}
	}
	
	private void animationUpdate() {
		Vector3 temp = facing;
		temp.y = 0.0f;
		if (temp != Vector3.zero) {
			animator.SetBool("Moving", true);
			// facing = rigidbody.velocity.normalized;
			transform.rotation = Quaternion.LookRotation(facing);
		} else {
			animator.SetBool("Moving", false);
		}
		
	}
	
	// Checks if user is grounded by sending a raycast to the ground
	// If called multiple times per update, consider making a bool variable and only call once per updaye
	public bool isGrounded () {
		return Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
	}
	
	/****************************************************************/
}
