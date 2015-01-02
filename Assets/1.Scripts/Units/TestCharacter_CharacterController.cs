using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class TestCharacter_CharacterController : MonoBehaviour {

	public float speed = 100.0f;
	public float gravitySpeed = 3.0f;
	public float rotateSpeed = 3.0f;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	// Use this for initialization
	void Start () {
		 controller = GetComponent<CharacterController>();
	}
	
	void FixedUpdate() {
		moveCommands ();
		gravityEffect ();
		moveDirection = transform.TransformDirection(moveDirection);
		controller.Move(moveDirection * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
		// transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
	}
	
	/* Seperate these functions into an interface/superclass later */
	
	void moveCommands() {
		if (controller.isGrounded) {
			float totalDir = Mathf.Abs (Input.GetAxis("Horizontal")) + Mathf.Abs (Input.GetAxis ("Vertical"));
			if (totalDir == 0) totalDir = 1;
			moveDirection = new Vector3(Input.GetAxis("Horizontal")/totalDir, 0, Input.GetAxis("Vertical")/totalDir);
			moveDirection *= speed;
			print (moveDirection.x + ", " + moveDirection.z);
		}
	}
	
	
	void gravityEffect() {
		moveDirection.y -= gravitySpeed * Time.deltaTime;

	}
	
	/****************************************************************/
}
