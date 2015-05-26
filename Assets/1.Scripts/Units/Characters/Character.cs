// The class that will takeover the character class we have new models and animations for everyhing

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Stats{
	//Base Stats
	public int health, strength, coordination, armor, armorBonus;
	public float speed;
	[HideInInspector] public int maxHealth, rezCount;
	
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
	
	public Stats() {
		dmgManip = new DamageManipulation();
		spdManip = new SpeedManipulation();
	}
}

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IDamageable<int, Transform, GameObject>, IStunable, IForcible<Vector3, float> {

	public bool lockRotation = false;
	
	public bool testControl;
	
	public bool isDead = false;

	public bool isHit = false;
	
	public Vector3 facing; // Direction unit is facing
	
	
	public bool actable, freeAnim, attacking, stunned, knockedback, animationLock;
	public AudioClip hurt, victory, failure;
	
	public bool testing; // Whether it takes gear in automatically or lets the gear loader to it
	
	public bool playSound;
	
	public bool invincible = false;
	public GameObject drop;
	public GameObject splatter;
	public Collider col;
	public Rigidbody rb;
	public Type opposition;
	public Renderer[] rs;
	public GameObject expDeath;
	public Knockback hitConfirm;
	
	// Animation variables
	public Animator animator;
	
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
	
	
	//-------------------//
	// Primary Functions //
	//-------------------//
	
	protected virtual void Awake() {
		opposition = Type.GetType ("Player");
		BDS = new BuffDebuffSystem(this);
		stats = new Stats();
		this.animator = GetComponent<Animator>();
		this.rb = this.GetComponent<Rigidbody>();
		this.col = this.GetComponent<Collider>();

		facing = Vector3.forward;
		isDead = false;
		stunned = knockedback = animationLock = false;
		setInitValues();
		this.testControl = true;
	}
	
	// Use this for initialization
	protected virtual void Start () {
		if (testing) {
			this.SetGearAndAbilities();
		}
		foreach (CharacterBehaviour behaviour in this.animator.GetBehaviours<CharacterBehaviour>()) {
			behaviour.SetVar(this);
		}

	}
	
	protected virtual void setInitValues() {
		playSound = true;
	}

	public virtual void SetGearAndAbilities() {
		gear.equipGear(this, opposition);
		inventory.equipItems(this, opposition);
	}

	public virtual void equipTest(GameObject[] equip, GameObject[] abilities) {
		gear.equipGear(this, opposition, equip);
		inventory.equipItems(this, opposition, abilities);
	}

	// Update is called once per frame
	protected virtual void Update () {
		if(isDead) return;

		freeAnim = !stunned && !knockedback && !animationLock;
		actable = freeAnim;
		
		this.animator.SetBool("Actable", this.actable);
		
		AnimationUpdate ();
	}
	
	//---------------------------------//
	// Action interface implementation //
	//---------------------------------//


	protected virtual void ActionCommands() {
		
	}
	
	// Constant animation updates (Main loop for characters movement/actions)
	protected virtual void AnimationUpdate() {
		// print(this.rb.velocity);
		if (this.rb.velocity != Vector3.zero && facing != Vector3.zero) animator.SetBool("Moving", true);
		else animator.SetBool("Moving", false);
		transform.localRotation = Quaternion.LookRotation(facing);
	}
	//-------------------------------------------//

	
	//------------------------------------//
	// Attacking Function Implementations //
	//------------------------------------//
	
	public virtual void AttackStart() {
		this.gear.weapon.AttackStart ();
	}
	
	public virtual void AttackEnd() {
		this.gear.weapon.AttackEnd ();
	}

	public virtual void SpecialAttack() {
		this.gear.weapon.SpecialAttack();
	}
	
	//------------------------------------//
	
	
	
	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public virtual void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		this.damage (dmgTaken, atkPosition); // Untill character needs to do something with source, just call previous function
	}
	
	public virtual void damage(int dmgTaken, Transform atkPosition) {
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
	
	public virtual void damage(int dmgTaken) {
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
	public virtual void die() {
		isDead = true;
		actable = false;
		freeAnim = false;
		Debug.Log ("should be displaying 2nd");
		//GetComponent<Collider> ().isTrigger = true;
	}
	
	public virtual void rez(){
		if(isDead){
			isDead = false;
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
	
	//-------------------------------//
	// Stun Interface Implementation //
	//-------------------------------//
	
	public virtual bool stun() {
		animator.SetBool("Charging", false);
		this.stunned = true;
		this.rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		return true;
	}
	
	public virtual void removeStun() {
		this.stunned = false;
	}
	
	//-------------------------------//
	
	
	//--------------------------------//
	// Force Interface Implementation //
	//--------------------------------//
	
	public virtual bool knockback(Vector3 direction, float speed) {
		animator.SetBool("Charging", false);
		this.knockedback = true;
		this.rb.velocity = direction.normalized * speed;
		return true;
	}
	
	public virtual void stabled() {
		this.rb.velocity = Vector3.zero;
		this.knockedback = false;
	}
	
	// The duration are essentiall y stun, expand on these later
	public virtual void pull(float pullDuration) {
		stun();
	}
	
	public virtual void push(float pushDuration) {
		stun();
	}
	
	//--------------------------------//

	protected virtual void deathNoise(){	
		StartCoroutine (makeSound (failure, playSound, failure.length));
	}

	public virtual IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position, 0.5f);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
	

}