// Parent script for player controlled characters

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Controls {
	//First 7 are Keys, last 2 are joystick axis, attackJoystick
	public string up, down, left, right, attack, attkJoy, secItem, cycItem, hori, vert;
	//0 for Joystick off, 1 for Joystick on and no keys
	public int joyUsed;
}

[System.Serializable]
public class Stats{
	//Base Stats
	public int health, armor, strength, coordination, speed, luck;
	
	[Range(0.5f, 2.0f)]
	public float atkSpeed;
	/*
	*Health: Health is the amount of damage a player can take before dying.
	*Armor: Effects the amount of health that is lost when a player is hit with an attack, the higher the armor the less health is lost.
	*Strength: The measure of how effective a player is with melee weapons. May also affect carrying speed of large objects involved in puzzles.
	*Coordination: The measure of how effective a player is with ranged weapons. May also affect other relevant puzzle elements, like rewiring or lock picking. Influences reload time(reload has a cap).
	*Speed: Affects the player's movement speed and recovery times after attacks. (this should have a cap)
	*Luck: Affects the players chances at success in whatever they do. Gives players a higher critical strike chance in combat and otherwise (if relevant).
	*/

}

[System.Serializable]
public class Equip{
	//Name of item (first item, second item, third item)
	public GameObject weapon, helmet, bodyArmor;
	//1 = Sword, 2 = Gun, 3 = Flamethrower
	public int weapType;
}

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IActionable, IMoveable, IFallable, IAttackable, IDamageable<int> {
	
	public float speed = 5.0f;
	public float gravity = 50.0f;
	public bool isGrounded = false;
	public bool actable = true; // Boolean to show if a unity can act or is stuck in an animation
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up
	
	public Controls controls;
	public Stats stats;
	public Equip equip;
	public float freeAnim;

	// Atk action variables
	protected float chgAtkTime = -1;
	protected float chgDuration = 0;
	protected float maxChgTime = 2.0f; // Put into weapon later

	// Animation variables
	protected Animator animator;
	protected AnimatorStateInfo animSteInfo;
	protected int idleHash, runHash, atkHash;
	
	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator>();
		facing = Vector3.forward;
		setAnimHash();
	}

	// Gets hash code for animations (Faster than using string name when running)
	protected virtual void setAnimHash() {
		idleHash = Animator.StringToHash ("Base Layer.idle");
		runHash = Animator.StringToHash ("Base Layer.run");
		atkHash = Animator.StringToHash ("Base Layer.attack");
	}
	
	protected virtual void FixedUpdate() {

	}
	
	// Update is called once per frame
	protected virtual void Update () {
		isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);

		animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
		actable = animSteInfo.nameHash == runHash || animSteInfo.nameHash == idleHash;

		if (isGrounded) {
			actionCommands ();
			moveCommands ();
		} else {
			falling();
		}

		animationUpdate ();
	}

	//---------------------------------//
	// Action interface implementation //
	//---------------------------------//

	public virtual void actionCommands() {
		// Invokes an action/animation
		if (actable) {
			if(Input.GetKeyDown(controls.attack)) {
				chgAtkTime = chgDuration = 0;
				animator.SetTrigger("Attack");
			}
		// Continues with what is happening
		} else {
			if (animSteInfo.nameHash == atkHash) {
				attacks();
			}
		}
	}

	// Constant animation updates (Main loop for characters movement/actions)
	public virtual void animationUpdate() {
		Vector3 temp = facing;
		temp.y = 0.0f;
		if (animSteInfo.nameHash == atkHash) {
			attackAnimation(temp);
		} else {
			movementAnimation(temp);
		}
	}

	//-------------------------------------------//

	// Animation helper functions
	protected virtual void attackAnimation(Vector3 tFacing) {
		if (chgAtkTime > 0) {
			if(animSteInfo.normalizedTime < .33) animator.speed = stats.atkSpeed;
			else animator.speed = 0;
			if (tFacing != Vector3.zero) transform.localRotation = Quaternion.LookRotation(facing);
		} else if (chgAtkTime == -1) {
			animator.speed = stats.atkSpeed;
		}
		
		// Weapon Collider information (Put it into the weapons themselves in the future)
		if(equip.weapType == 1){
		if(animSteInfo.normalizedTime < .33 || animSteInfo.normalizedTime > .7) {
			
			equip.weapon.GetComponent<Collider>().enabled = false;
		} else {
			equip.weapon.GetComponent<Collider>().enabled = true;
		}
		}
	}

	protected virtual void movementAnimation(Vector3 tFacing) {
		animator.speed = 1; // Change animation speed back for other animations
		if (tFacing != Vector3.zero) {
			animator.SetBool("Moving", true);
			transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			animator.SetBool("Moving", false);
		}
	}


	//-----------------------------------//
	// Movement interface implementation //
	//-----------------------------------//
	
	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;

		if (actable || chgAtkTime > 0) { // Replace animator with something less specific as we get more animatons/actions in
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
			
			rigidbody.velocity = facing.normalized * speed;
		} else {
			// Right now this stops momentum when performing an action
			// If we trash the rigidbody later, we won't need this
			rigidbody.velocity = Vector3.zero;
		}
	}

	//-------------------------------------//


	//----------------------------------//
	// Falling Interface Implementation //
	//----------------------------------//w

	public virtual void falling() {
		// fake gravity
		// Animation make it so rigidbody gravity works oddly due to some gravity weight
		// Seems like Unity Pro is needed to change that, so unless we get it, this will suffice 
		rigidbody.velocity = new Vector3 (0.0f, -gravity, 0.0f);
	}

	//----------------------------------//


	//---------------------------------//
	// Attack Interface Implementation //
	//---------------------------------//

	// If using a basic attack, this will do checks (such as charging an attack)
	public virtual void attacks() {
		if (!Input.GetKey(controls.attack) && chgAtkTime != -1) {
			chgAtkTime = -1;
			if(equip.weapType == 2){
				equip.weapon.GetComponent<Gun>().bullet.transform.rotation = transform.rotation;
				equip.weapon.GetComponent<Weapons>().particles.startSpeed = (int)(chgDuration/0.4f);
				Instantiate(equip.weapon.GetComponent<Gun>().bullet, transform.position, equip.weapon.GetComponent<Gun>().bullet.transform.rotation);
			}
			print("Charge Attack power level:" + (int)(chgDuration/0.4f));
		} else if (chgAtkTime == 0 && animSteInfo.normalizedTime > .32) {
			chgAtkTime = Time.time;
			equip.weapon.GetComponent<Weapons>().particles.startSpeed = 0;
			if(equip.weapType == 2){
				equip.weapon.GetComponent<Gun>().bullet.transform.rotation = transform.rotation;
				equip.weapon.GetComponent<Weapons>().particles.startSpeed = (int)(chgDuration/0.4f);
			}
			equip.weapon.GetComponent<Weapons>().particles.Play();
		} else if (chgAtkTime != -1 && animSteInfo.normalizedTime > .32) {
			chgDuration = Mathf.Clamp(Time.time - chgAtkTime, 0.0f, maxChgTime);
			equip.weapon.GetComponent<Weapons>().particles.startSpeed = (int)(chgDuration/0.4f);
		}

		if (animSteInfo.normalizedTime > .7) {
			equip.weapon.GetComponent<Weapons>().particles.Stop();
		}
	}
	
	//---------------------------------//


	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public virtual void damage(int dmgTaken) {
		print ("OWW, WTF DUDE");
	}

	//----------------------------------//
}