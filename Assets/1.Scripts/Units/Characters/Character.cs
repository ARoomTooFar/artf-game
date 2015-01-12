// Parent script for player controlled characters

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public int health, armor;
	[Range(0,10)]
	public int strength, coordination, speed, luck;
	
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
	//Name of item
	public Weapons weapon;
	public Equipment helmet, bodyArmor;

	public void equipItems(Character curPlayer) {
		if (weapon) weapon.player = curPlayer;
	}
}

[System.Serializable]
public class CharItems {
	public int selected;
	public List<Item> items = new List<Item>();

	public void equipItems(Character curPlayer) {
		for (int i = 0; i < items.Count; i++)
			items[i].player = curPlayer;
	}

	public void cycItems() {
		if (++selected >= items.Count) selected = 0;
	}
}

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IActionable, IMoveable, IFallable, IAttackable, IDamageable<int> {
	
	public float speed = 5.0f;
	public float gravity = 50.0f;
	public bool isGrounded = false;
	public bool actable = true; // Boolean to show if a unit can act or is stuck in an animation
	
	public Vector3 facing; // Direction unit is facing
	public Vector3 curFacing; // A better facing var, will change and combine in future
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up
	
	public Controls controls;
	public Stats stats;
	public CharItems charItems;

	public bool freeAnim;

	// Animation variables
	public Animator animator;
	public AnimatorStateInfo animSteInfo;
	protected int idleHash, runHash, atkHash;
	
	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator>();
		facing = curFacing = Vector3.forward;
		freeAnim = true;
		stats.equipItems(this);
		charItems.equipItems(this);
		setAnimHash();
	}
	protected virtual void setInitValues() {
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
		actable = (animSteInfo.nameHash == runHash || animSteInfo.nameHash == idleHash) && freeAnim;

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
				stats.weapon.initAttack();
				// chgAtkTime = chgDuration = 0;
				// animator.SetTrigger("Attack");
			} else if(Input.GetKeyDown (controls.secItem)) {
				if (charItems.items.Count > 0 && charItems.items[charItems.selected].curCoolDown <= 0) {
					charItems.items[charItems.selected].useItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
				} else {
					// Play sound for trying to use item on cooldown or items
					print("Item on Cooldown");
				}
			} else if(Input.GetKeyDown (controls.cycItem)) {
				charItems.cycItems();
			}
		// Continues with what is happening
		} else {
			if (animSteInfo.nameHash == atkHash) {
				attacks();
			} 
			/*else if (animSteInfo.nameHash == rollHash) { for later
			}
			*/
		}


		if (Input.GetKeyUp (controls.secItem))  {
			if (charItems.items.Count > 0) {
				charItems.items[charItems.selected].deactivateItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
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
		// Should this also be in the weapons

		if (stats.weapon.stats.curChgAtkTime > 0) {
			if(animSteInfo.normalizedTime < stats.weapon.stats.colStart) animator.speed = stats.atkSpeed;
			else animator.speed = 0;
			if (tFacing != Vector3.zero) transform.localRotation = Quaternion.LookRotation(facing);
		} else if (stats.weapon.stats.curChgAtkTime == -1) {
			animator.speed = stats.weapon.stats.atkSpeed;
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

		if (actable || stats.weapon.stats.curChgAtkTime > 0) { // Better Check here
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
			if (facing != Vector3.zero)
				curFacing = facing;
			
			rigidbody.velocity = facing.normalized * speed;
		} else if (freeAnim){
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
		stats.weapon.attack ();
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