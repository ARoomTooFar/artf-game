// Parent script for player controlled characters

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Controls {
	public string up, down, left, right;
}

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IMoveable {

	public float speed = 5.0f;
	public float gravity = 50.0f;
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up
	
	public Controls controls;
	
	protected Animator animator;

	// Use this for initialization
	protected virtual void Start () {
		animator = this.GetComponent<Animator>();
		facing = Vector3.forward;
	}
	
	protected virtual void FixedUpdate() {
		moveCommands ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		animationUpdate ();
	}
	
	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;
		if (isGrounded()) {
			// Up is pressed
			if (Input.GetKey(controls.up)) {
				newMoveDir += Vector3.forward;
			}
			
			if (Input.GetKey(controls.down)) {
				newMoveDir += Vector3.back;
			}
			
			if (Input.GetKey(controls.left)) {
				newMoveDir += Vector3.left;
			}
			
			if (Input.GetKey(controls.right)) {
				newMoveDir += Vector3.right;
			}
			
			facing = newMoveDir;
			
			// Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			
			rigidbody.velocity = facing.normalized * speed;
		} else {
			// fake gravity
			// Animation make it so rigidbody gravity works oddly due to some gravity weight
			// Seems like Unity Pro is needed to change that, so unless we get it, this will suffice 
			rigidbody.velocity = new Vector3 (0.0f, -gravity, 0.0f);
		}
	
		// Old movement Code
		/*
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
		*/
	}
	
	// Constant animation updates (Mainlor characters movement/actions)
	public virtual void animationUpdate() {
		Vector3 temp = facing;
		temp.y = 0.0f;
		if (temp != Vector3.zero) {
			animator.SetBool("Moving", true);
			transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			animator.SetBool("Moving", false);
		}
		
	}
	
	// Checks if user is grounded by sending a raycast to the ground
	// If called multiple times per update, consider making a bool variable and only call once per update
	public virtual bool isGrounded () {
		return Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
	}
}
