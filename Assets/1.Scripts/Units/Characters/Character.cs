// Parent script for player controlled characters

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Controls {
	//First 7 are Keys, last 2 are joystick axis
	public string up, down, left, right, attack, secItem, cycItem, hori, vert;
	//0 for Joystick off, 1 for Joystick on and no keys
	public int joyUsed;
}

[System.Serializable]
public class Stats{
	//Base Stats
	public int health, armor, strength, coordination, speed, luck;

	[Range(0, 100)]
	public int atkSpeed;
	/*
	*Health: Health is the amount of damage a player can take before dying.
	*Armor: Effects the amount of health that is lost when a player is hit with an attack, the higher the armor the less health is lost.
	*Strength: The measure of how effective a player is with melee weapons. May also affect carrying speed of large objects involved in puzzles.
	*Coordination: The measure of how effective a player is with ranged weapons. May also affect other relevant puzzle elements, like rewiring or lock picking. Influences reload time(reload has a cap).
	*Speed: Affects the player's movement speed and recovery times after attacks. (this should have a cap)
	*Luck: Affects the players chances at success in whatever they do. Gives players a higher critical strike chance in combat and otherwise (if relevant).
	*/
	//Name of item
	public GameObject weapon, helmet, bodyArmor;
}

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IMoveable, IAttackable, IDamageable<int> {

	public float speed = 5.0f;
	public float gravity = 50.0f;
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up

	public Controls controls;
	public Stats stats;
	
	protected Animator animator;

	// Use this for initialization
	protected virtual void Start () {
		animator = this.GetComponent<Animator>();
		facing = Vector3.forward;
	}
	
	protected virtual void FixedUpdate() {
		actionCommands ();
		moveCommands ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		animationUpdate ();
	}

	public virtual void actionCommands() {
		if (isGrounded ()) {
			if(Input.GetKeyDown(controls.attack)) {
				animator.speed = stats.atkSpeed;
				animator.SetTrigger("Attack");
			} else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack")) {
				if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .33 || animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .75) {
					stats.weapon.GetComponent<Collider>().enabled = false;
				} else {
					stats.weapon.GetComponent<Collider>().enabled = true;
				}
			}
		}
	}

	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;

		if (isGrounded() && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack")) { // Replace animator with something less specific as we get more animatons/actions in
			//"Up" key assign pressed
			if (Input.GetKey(controls.up)) {
				newMoveDir += Vector3.forward;
			}
			//"Down" key assign pressed
			if (Input.GetKey(controls.down)) {
				newMoveDir += Vector3.back;
			}
			//"Left" key assign pressed
			if (Input.GetKey(controls.left)) {
				newMoveDir += Vector3.left;
			}
			//"Right" key assign pressed
			if (Input.GetKey(controls.right)) {
				newMoveDir += Vector3.right;
			}
			//Joystick form
			if(controls.joyUsed == 1){
				newMoveDir = new Vector3(Input.GetAxis(controls.hori),0,Input.GetAxis(controls.vert));
			}
			facing = newMoveDir;
			
			// Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			
			rigidbody.velocity = facing.normalized * speed;
		}else {
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

	public virtual void damage(int dmgTaken) {
		print ("OWW, WTF DUDE");
	}

	// Checks if user is grounded by sending a raycast to the ground
	// If called multiple times per update, consider making a bool variable and only call once per update
	public virtual bool isGrounded () {
		return Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
	}
}
