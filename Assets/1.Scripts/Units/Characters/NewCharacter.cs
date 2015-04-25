// The class that will takeover the character class we have new models and animations for everyhing

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
public class NewCharacter : Character {//MonoBehaviour, IActionable<bool>, IFallable, IAttackable, IDamageable<int, Transform, GameObject>, IStunable, IForcible<Vector3, float> {

	/*
	public bool testControl;
	
	public float gravity = 50.0f;
	public bool isDead = false;
	public bool isGrounded = false;
	public bool actable = true; // Boolean to show if a unit can act or is stuck in an animation
	
	public Vector3 facing; // Direction unit is facing
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up
	
	public bool freeAnim, attacking, stunned, knockedback;
	public AudioClip hurt, victory, failure;
	
	public bool testing, invis; // Whether it takes gear in automatically or lets the gear loader to it
	
	public bool invincible = false;
	public GameObject drop;
	public GameObject splatter;
	public Collider col;
	public Rigidbody rb;
	public Type opposition;
	public Renderer[] rs;
	public Cloak[] skins;
	public GameObject expDeath;
	
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
				// Debug.LogWarning(player.gameObject.name + " does not have a weapon in the weapon slot.");
			}
			
			helmet = headLocation.GetComponentInChildren<Helmet>();
			if (helmet) {
				helmet.equip (player);
			} else {
				// Debug.LogWarning(player.gameObject.name + " does not have a helmet in the helmet slot.");
			}
			
			chest = chestLocation.GetComponentInChildren<Chest>();
			if (chest) {
				chest.equip (player);
			} else {
				//Debug.LogWarning(player.gameObject.name + " does not have armor in the armor slot.");
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
		
		public void equipItems(Character player, Type ene, GameObject[] abilities) {
			foreach (GameObject item in abilities) {
				Item newItem = (Instantiate(item) as GameObject).GetComponent<Item>();
				newItem.transform.SetParent(itemLocation, false);
				newItem.user = player;
				newItem.opposition = ene;
				items.Add(newItem);
			}
			
			selected = 0;
			keepItemActive = false;
		}
		
		// Equip method for testing purposes
		public void equipItems(Character player, Type ene) {
			items.Clear ();
			items.AddRange(itemLocation.GetComponentsInChildren<Item>());
			
			if (items.Count == 0) {
				Debug.LogWarning(player.gameObject.name + " does not have any abilities in the item slot.");
			}
			
			foreach (Item item in items) {
				item.user = player;
				item.opposition = ene;
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

	*/
	
	protected override void Awake() {
		opposition = Type.GetType ("Player");
		BDS = new BuffDebuffSystem(this);
		stats = new Stats();
		this.animator = GetComponent<Animator>();
		this.rb = this.GetComponent<Rigidbody>();
		this.col = this.GetComponent<Collider>();

		facing = Vector3.forward;
		isDead = false;
		stunned = knockedback = false;
		setInitValues();
		this.testControl = true;
		skins = gameObject.GetComponentsInChildren<Cloak>();
	}
	
	// Use this for initialization
	protected override void Start () {
		if (testing) this.SetGearAndAbilities();
		foreach (CharacterBehaviour behaviour in this.animator.GetBehaviours<CharacterBehaviour>()) {
			behaviour.SetVar(this);
		}

	}
	
	protected override void setInitValues() {
		
	}

	public virtual void SetGearAndAbilities() {
		gear.equipGear(this, opposition);
		inventory.equipItems(this, opposition);
	}

	public override void equipTest(GameObject[] equip, GameObject[] abilities) {
		gear.equipGear(this, opposition, equip);
		inventory.equipItems(this, opposition, abilities);
	}

	// Update is called once per frame
	protected override void Update () {
		if(isDead) return;


		freeAnim = !stunned && !knockedback;
		actable = freeAnim;
		this.animator.SetBool("Actable", this.actable);

		// actionCommands ();
		animationUpdate ();
	}
	
	//---------------------------------//
	// Action interface implementation //
	//---------------------------------//
	
	public override void actionCommands() {
		
	}
	
	// Constant animation updates (Main loop for characters movement/actions)
	public override void animationUpdate() {
		// print(this.rb.velocity);
		if (this.rb.velocity != Vector3.zero && facing != Vector3.zero) animator.SetBool("Moving", true);
		else animator.SetBool("Moving", false);
		transform.localRotation = Quaternion.LookRotation(facing);
	}
	//-------------------------------------------//
	
	
	//---------------------------------//
	// Attack Interface Implementation //
	//---------------------------------//
	
	// Since animations are on the characters, we will use the attack methods to turn collisions on and off
	public override void initAttack() {
	}
	
	public override void attacks() {
		
	}
	
	
	public override void colliderStart() {
		gear.weapon.collideOn ();
	}
	
	public override void colliderEnd() {
		gear.weapon.collideOff ();
	}
	
	public override void specialAttack() {
		gear.weapon.specialAttack ();
	}
	
	//---------------------------------//
	
	
	
	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		this.damage (dmgTaken, atkPosition); // Untill character needs to do something with source, just call previous function
	}
	
	public override void damage(int dmgTaken, Transform atkPosition) {
		if (!invincible) {
			dmgTaken = Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(atkPosition.position, facing, transform.position)), 1, 100000);
			if(splatter != null){
				splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position, Quaternion.identity)).GetComponent<splatCore>();
				theSplat.adjuster = (float) dmgTaken/stats.maxHealth;
				Destroy (theSplat, 2);
			}
			stats.health -= dmgTaken;
			
			if (stats.health <= 0) this.die();
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible) {
			if(splatter != null){
				splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position, Quaternion.identity)).GetComponent<splatCore>();
				theSplat.adjuster = (float) dmgTaken/stats.maxHealth;
				Destroy (theSplat, 2);
			}
			stats.health -= dmgTaken;
			
			if (stats.health <= 0) die();
		}
	}
	
	// Add logic to this in the future
	//     ie: Removing actions, player from camera etc
	public override void die() {
		stats.isDead = true;
		actable = false;
		freeAnim = false;
	}
	
	public override void rez(){
		if(stats.isDead){
			stats.isDead = false;
			stats.health = stats.maxHealth/(2+2*stats.rezCount);
			stats.rezCount++;
		}else{
			heal(stats.maxHealth/(2+2*stats.rezCount));
		}
	}
	public override void heal(int healTaken){
		if(stats.health < stats.maxHealth){
			stats.health+=healTaken;
			if(stats.health > stats.maxHealth){
				stats.health = stats.maxHealth;
			}
		}
	}
	
	//-------------------------------//
	
	//-------------------------------//
	// Stun Interface Implementation //
	//-------------------------------//
	
	public override bool stun() {
		animator.SetBool("Charging", false);
		this.stunned = true;
		this.rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		return true;
	}
	
	public override void removeStun() {
		this.stunned = false;
	}
	
	//-------------------------------//
	
	
	//--------------------------------//
	// Force Interface Implementation //
	//--------------------------------//
	
	public override bool knockback(Vector3 direction, float speed) {
		animator.SetBool("Charging", false);
		this.knockedback = true;
		this.rb.velocity = direction.normalized * speed;
		return true;
	}
	
	public override void stabled() {
		this.rb.velocity = Vector3.zero;
		this.knockedback = false;
	}
	
	// The duration are essentiall y stun, expand on these later
	public override void pull(float pullDuration) {
		stun();
	}
	
	public override void push(float pushDuration) {
		stun();
	}
	
	//--------------------------------//
	
}