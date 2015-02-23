// Parent script for player controlled characters

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Stats{
	//Base Stats
	public int health, armor,maxHealth,rezCount;
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

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IActionable<bool>, IFallable, IAttackable, IDamageable<int, Character>, IStunable, IForcible<float> {

	public float gravity = 50.0f;
	public bool isDead = false;
	public bool isGrounded = false;
	public bool actable = true; // Boolean to show if a unit can act or is stuck in an animation
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up

	public bool freeAnim, attacking;
	public AudioClip hurt, victory, failure;

	public bool testing; // Whether it takes gear in automatically or lets the gear loader to it

	public bool invincible = false;

	protected Type opposition;
	
	// Animation variables
	public Animator animator;
	public AnimatorStateInfo animSteInfo;
	
	// Swap these over to weapons in the future
	public string weapTypeName;
	public int idleHash, runHash, atkHashStart, atkHashCharge, atkHashSwing, atkHashChgSwing, atkHashEnd, animSteHash;

	// protected delegate void BuffDelegate(float strength);

	public BuffDebuffSystem BDS;

	// Serialized classes
	public Stats stats;

	public Gear gear;
	[System.Serializable]
	public class Gear {
		public Weapons weapon;
		public Helmet helmet;
		public Chest chest;
		public Transform weapLocation, headLocation, chestLocation;
		
		public void equipGear(Character player, Type ene, GameObject[] equipment) {
			foreach (GameObject equip in equipment) {
				if (equip.GetComponent<Weapons>()) {
					// GameObject newGear = Instantiate(equip, headLocation.position, headLocation.rotation) as GameObject;
					weapon = (Instantiate(equip) as GameObject).GetComponent<Weapons>();
					weapon.transform.SetParent(weapLocation, false);
					weapon.equip(player, ene);
				} else if (equip.GetComponent<Helmet>()) {
					helmet = (Instantiate(equip) as GameObject).GetComponent<Helmet>();
					helmet.transform.SetParent(headLocation, false);
					helmet.equip(player);
				} else if (equip.GetComponent<Chest>()) {
					chest = (Instantiate(equip) as GameObject).GetComponent<Chest>();
					chest.transform.SetParent(chestLocation, false);
					chest.equip(player);
				} else {
					Debug.LogWarning("Non-weapon/armor passed into gear class");
				}
			}
		}

		// Equip method for testing purposes
		public void equipGear(Character player, Type ene) {
			weapon = weapLocation.GetComponentInChildren<Weapons>();
			if (weapon) {
				weapon.equip (player, ene);
			} else {
				Debug.LogWarning(player.gameObject.name + " does not have a weapon in the weapon slot.");
			}

			helmet = headLocation.GetComponentInChildren<Helmet>();
			if (helmet) {
				helmet.equip (player);
			} else {
				Debug.LogWarning(player.gameObject.name + " does not have a helmet in the helmet slot.");
			}

			chest = chestLocation.GetComponentInChildren<Chest>();
			if (chest) {
				chest.equip (player);
			} else {
				Debug.LogWarning(player.gameObject.name + " does not have armor in the armor slot.");
			}
		}
	}


	public Inventory inventory;
	// might move to player depending on enemy stuff or have each class also have an inventory class inheriting this inventory
	[System.Serializable]
	public class Inventory {
		public int selected;
		public bool keepItemActive;
		public Transform itemLocation;
		
		public List<Item> items = new List<Item>();
		
		public void equipItems(Character player, GameObject[] abilities) {
			foreach (GameObject item in abilities) {
				Item newItem = (Instantiate(item) as GameObject).GetComponent<Item>();
				newItem.transform.SetParent(itemLocation, false);
				newItem.user = player;
				items.Add(newItem);
			}
				
			selected = 0;
			keepItemActive = false;
		}
		
		// Equip method for testing purposes
		public void equipItems(Character player) {
			items.Clear ();
			items.AddRange(itemLocation.GetComponentsInChildren<Item>());

			if (items.Count == 0) {
				Debug.LogWarning(player.gameObject.name + " does not have any abilities in the item slot.");
			}

			foreach (Item item in items) {
				item.user = player;
			}
			
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

	protected virtual void Awake() {
		opposition = Type.GetType ("Player");
		BDS = new BuffDebuffSystem(this);
		stats = new Stats(this.GetComponent<MonoBehaviour>());
		animator = GetComponent<Animator>();
		facing = Vector3.forward;
		isDead = false;
		freeAnim = true;
		setInitValues();
	}

	// Use this for initialization
	protected virtual void Start () {
		if (testing) {
			gear.equipGear(this, opposition);
			inventory.equipItems(this);
			setAnimHash();
		}
	}
	
	protected virtual void setInitValues() {

	}

	public virtual void equipTest(GameObject[] equip, GameObject[] abilities) {
		gear.equipGear(this, opposition,equip);
		inventory.equipItems(this, abilities);
		setAnimHash();
	}

	// Gets hash code for animations (Faster than using string name when running)
	protected virtual void setAnimHash() {
		idleHash = Animator.StringToHash ("Base Layer.idle");
		runHash = Animator.StringToHash ("Base Layer.run");
		
		// atkHash = Animator.StringToHash ("Base Layer.attack");
		atkHashStart = Animator.StringToHash (weapTypeName + "." + weapTypeName + "Start");
		atkHashCharge = Animator.StringToHash (weapTypeName + "." + weapTypeName + "Charge");
		atkHashSwing = Animator.StringToHash (weapTypeName + "." + weapTypeName + "Swing");
		atkHashChgSwing = Animator.StringToHash (weapTypeName + "." + weapTypeName + "ChargedSwing");
		atkHashEnd = Animator.StringToHash (weapTypeName + "." + weapTypeName + "End");
	}
	
	protected virtual void FixedUpdate() {

	}
	
	// Update is called once per frame
	protected virtual void Update () {
	    if(!isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);

			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.nameHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd ;

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

	public virtual void setActable(bool canAct) {
		actable = canAct;
	}

	public virtual void actionCommands() {

	}

	// Constant animation updates (Main loop for characters movement/actions)
	public virtual void animationUpdate() {
		if (attacking) {
			attackAnimation();
		} else {
			movementAnimation();
		}
	}
	//-------------------------------------------//

	// Animation helper functions
	protected virtual void attackAnimation() {
		// Should this also be in the weapons?
	
		/*
		if (gear.weapon.stats.curChgAtkTime > 0) {
			// Change once we get animations
			if(animSteInfo.normalizedTime < gear.weapon.stats.colStart) animator.speed = gear.weapon.stats.atkSpeed;
			else animator.speed = 0;
			if (rigidbody.velocity != Vector3.zero) transform.localRotation = Quaternion.LookRotation(facing);
		} else if (gear.weapon.stats.curChgAtkTime == -1) {
			animator.speed = gear.weapon.stats.atkSpeed;
		}*/
	}

	protected virtual void movementAnimation() {
		// animator.speed = 1; // Change animation speed back for other animations
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

	// Since animations are on the characters, we will use the attack methods to turn collisions on and off
	public virtual void initAttack() {
	}

	public virtual void attacks() {

	}


	public virtual void colliderStart() {
		gear.weapon.collideOn ();
	}

	public virtual void colliderEnd() {
		gear.weapon.collideOff ();
	}

	public virtual void specialAttack() {
		gear.weapon.specialAttack ();
	}
	
	//---------------------------------//



	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public virtual void damage(int dmgTaken, Character striker) {
		if (!invincible) {
			Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(striker.transform.position, facing, transform.position)), 1, 100000);
		
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
	
	public virtual void rez(){
		Debug.Log("Dooby");
		if(stats.isDead){
			stats.isDead = false;
			stats.health = stats.maxHealth/(2+2*stats.rezCount);
			stats.rezCount++;
		}else{
			heal(stats.maxHealth/(2+2*stats.rezCount));
		}
	}
	public virtual void heal(int healTaken){
		if(stats.health < stats.maxHealth){
			stats.health+=healTaken;
			if(stats.health > stats.maxHealth){
				stats.health = stats.maxHealth;
			}
		}
	}
	
	//-------------------------------//

	/*
	//-------------------------------//
	// Slow Interface Implementation //
	//-------------------------------//
	//----------------------------------//
	public virtual void slow(float slowStrength) {
		stats.spdManip.setSpeedReduction(slowStrength);
	}

	public virtual void removeSlow(float slowStrength) {
		stats.spdManip.removeSpeedReduction(slowStrength);
	}

	public virtual void slowForDuration(float slowStrength, float slowDuration) {
		slow(slowStrength);
		StartCoroutine(buffTiming(slowStrength, slowDuration, removeSlow));
	}

	public virtual void speed(float speedStrength) {
		stats.spdManip.setSpeedAmplification(speedStrength);
	}
	
	public virtual void removeSpeed(float speedStrength) {
		stats.spdManip.removeSpeedAmplification(speedStrength);
	}
	
	public virtual void speedForDuration(float speedStrength, float speedDuration) {
		speed(speedStrength);
		StartCoroutine(buffTiming(speedStrength, speedDuration, removeSpeed));
	}

	//-------------------------------//
	*/

	//-------------------------------//
	// Stun Interface Implementation //
	//-------------------------------//
	
	public virtual bool stun() {
		this.freeAnim = false;
		this.rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		return true;
	}

	public virtual void removeStun() {
		this.freeAnim = true;
	}

	//-------------------------------//


	//--------------------------------//
	// Force Interface Implementation //
	//--------------------------------//
	
	// The duration are essentiall y stun, expand on these later
	public virtual void pull(float pullDuration) {
		stun();
	}
	
	public virtual void push(float pushDuration) {
		stun();
	}
	
	//--------------------------------//


	/*
	//-----------------------------//
	// Timing Event Implementation //
	//-----------------------------//

	// Used for buffs that are duration based
	// Uses delegates to call function when over
	// Will make virtual when neccessary
	protected virtual IEnumerator buffTiming(float strValue, float duration, BuffDelegate bd) {
		while (duration > 0) {
			duration -= Time.deltaTime;
			yield return null;
		}
		bd(strValue);
	}*/
}