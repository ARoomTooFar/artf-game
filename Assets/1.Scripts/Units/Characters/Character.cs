// Parent script for player controlled characters

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Stats{
	//Base Stats
	public int health, armor,maxHealth;
	public int strength, coordination, speed, luck;
	public bool isDead;
	/*
	*Health: Health is the amount of damage a player can take before dying.
	*Armor: Effects the amount of health that is lost when a player is hit with an attack, the higher the armor the less health is lost.
	*Strength: The measure of how effective a player is with melee weapons. May also affect carrying speed of large objects involved in puzzles.
	*Coordination: The measure of how effective a player is with ranged weapons. May also affect other relevant puzzle elements, like rewiring or lock picking. Influences reload time(reload has a cap).
	*Speed: Affects the player's movement speed and recovery times after attacks. (this should have a cap)
	*Luck: Affects the players chances at success in whatever they do. Gives players a higher critical strike chance in combat and otherwise (if relevant).
	*/
	public DamageManipulation dmgManip;
	public SpeedManipulation spdManip;

	public Stats(MonoBehaviour subMono) {
		dmgManip = new DamageManipulation(subMono);
		spdManip = new SpeedManipulation(subMono);
	}
}

[System.Serializable]
public class Gear {
	public Weapons weapon;
	public Equipment helmet, bodyArmor;

	public void equipItems(Character player) {
		if (weapon) weapon.equip(player);
	}
}

// might move to player depending on enemy stuff or have each class also have an inventory class inheriting this inventory
[System.Serializable]
public class Inventory {
	public int selected;
	public bool keepItemActive;
	public List<Item> items = new List<Item>();

	public void equipItems(Character curPlayer) {
		for (int i = 0; i < items.Count; i++)
			items[i].player = curPlayer;
		selected = 0;
		keepItemActive = false;
	}

	public void cycItems() {
		ToggleItem isToggle = items[selected].GetComponent<ToggleItem>();
		if (isToggle) {
			isToggle.deactivateItem();
		}
		selected = (selected + 1)%items.Count;
	}
}

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IActionable, IFallable, IAttackable, IDamageable<int, Character> {

	public float gravity = 50.0f;
	public bool isDead = false;
	public bool isGrounded = false;
	public bool actable = true; // Boolean to show if a unit can act or is stuck in an animation
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up
	
	public Controls controls;
	public Stats stats;
	public Gear gear;
	public Inventory inventory;
	public bool freeAnim;
	public AudioClip hurt, victory, failure;

	public bool invincible = false;

	// Animation variables
	public Animator animator;
	public AnimatorStateInfo animSteInfo;
	protected int idleHash, runHash, atkHash;
	
	// Use this for initialization
	protected virtual void Start () {
		stats = new Stats(this.GetComponent<MonoBehaviour>());
		animator = GetComponent<Animator>();
		facing = Vector3.forward;
		isDead = false;
		freeAnim = true;
		setInitValues();
		gear.equipItems(this);
		inventory.equipItems(this);
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
	    if(!isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);

			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			actable = (animSteInfo.nameHash == runHash || animSteInfo.nameHash == idleHash) && freeAnim;

			if (isGrounded) {
				actionCommands ();
			} else {
				falling();
			}

			animationUpdate ();
		}
	}

	//---------------------------------//
	// Action interface implementation //
	//---------------------------------//

	public virtual void actionCommands() {
		// Invokes an action/animation
		if (actable) {
			if(Input.GetKeyDown(controls.attack)) {
				gear.weapon.initAttack();
			} else if(Input.GetKeyDown (controls.secItem)) {
				if (inventory.items.Count > 0 && inventory.items[inventory.selected].curCoolDown <= 0) {
					inventory.keepItemActive = true;
					inventory.items[inventory.selected].useItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
				} else {
					// Play sound for trying to use item on cooldown or items
					print("Item on Cooldown");
				}
			} else if(Input.GetKeyDown (controls.cycItem)) {
				inventory.cycItems();
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
			if (inventory.items.Count > 0) {
				inventory.keepItemActive = false;
				// inventory.items[inventory.selected].deactivateItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
			}
		}
	}

	// Constant animation updates (Main loop for characters movement/actions)
	public virtual void animationUpdate() {
		if (animSteInfo.nameHash == atkHash) {
			attackAnimation();
		} else {
			movementAnimation();
		}
	}

	//-------------------------------------------//

	// Animation helper functions
	protected virtual void attackAnimation() {
		// Should this also be in the weapons?

		if (gear.weapon.stats.curChgAtkTime > 0) {
			// Change once we get animations
			if(animSteInfo.normalizedTime < gear.weapon.stats.colStart) animator.speed = gear.weapon.stats.atkSpeed;
			else animator.speed = 0;
			if (rigidbody.velocity != Vector3.zero) transform.localRotation = Quaternion.LookRotation(facing);
		} else if (gear.weapon.stats.curChgAtkTime == -1) {
			animator.speed = gear.weapon.stats.atkSpeed;
		}
	}

	protected virtual void movementAnimation() {
		animator.speed = 1; // Change animation speed back for other animations
		if (rigidbody.velocity != Vector3.zero) {
			animator.SetBool("Moving", true);
			transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			animator.SetBool("Moving", false);
		}
	}


	//----------------------------------//
	// Falling Interface Implementation //
	//----------------------------------//

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
		gear.weapon.attack ();
	}
	
	//---------------------------------//



	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public virtual void damage(int dmgTaken, Character striker) {
		if (!invincible) {
			Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(facing, transform.position, striker.transform.position)), 1, 100000);
		
			stats.health -= dmgTaken;
			
			if (stats.health <= 0) {
				die();
			}
		}
	}
	
	public virtual void damage(int dmgTaken) {
		if (!invincible) {
			stats.health -= dmgTaken;

			if (stats.health <= 0) {
				die();
			}
		}
	}

	// Add logic to this in the future
	//     ie: Removing actions, player from camera etc
	public virtual void die() {
		stats.isDead = true;
	}

	//----------------------------------//
}