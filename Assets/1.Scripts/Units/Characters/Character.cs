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
	public Type opposition;
	public Renderer[] rs;
	public GameObject expDeath;
	public Knockback hitConfirm;
	
	public Collider col;
	public Rigidbody rb;
	public Animator animator;
	
	public BuffDebuffSystem BDS;
	
	// Serialized classes
	public Stats stats;
	
	public Gear gear;
	[System.Serializable]
	public class Gear {
		public Weapons weapon;
		public Armor helmet;
		public Armor chest;
		public Transform weapLocation, headLocation, chestLocation;
		
		private Character unit;
		
		public void SetUnit (Character unit) {
			this.unit = unit;
		}
		
		public void EquipWeapon(GameObject weapon, Type ene, int tier) {
			this.weapon = (Instantiate(weapon) as GameObject).GetComponent<Weapons>();
			this.weapon.transform.SetParent(this.weapLocation, false);
			this.weapon.equip(this.unit, ene, tier);
		}
		
		public void EquipArmor(GameObject armor, int tier) {
			Chest chestPiece = chestLocation.GetComponentInChildren<Chest>();
			chestPiece.keyArmor = armor.GetComponent<SkinnedMeshRenderer>();
			
			chestPiece.Equip(this.unit, tier);
			this.chest = chestPiece.GetComponent<Armor>();
			//this.chest = (Instantiate(armor) as GameObject).GetComponent<Chest>();
			//this.chest.transform.SetParent(this.chestLocation, false);
			//this.chest.Equip(this.unit, tier);
		}
		
		public void EquipHelmet(GameObject helmet, int tier) {
			Helmet helmetPiece = headLocation.GetComponentInChildren<Helmet>();
			helmetPiece.keyHelmet = helmet.GetComponent<SkinnedMeshRenderer>();
			
			helmetPiece.Equip(this.unit, tier);
			this.helmet = helmetPiece.GetComponent<Armor>();
			// this.helmet = (Instantiate(helmet) as GameObject).GetComponent<Helmet>();
			// this.helmet.transform.SetParent(this.headLocation, false);
			// this.helmet.Equip(this.unit, tier);
		}
		
		// Equip method for testing purposes
		public void equipGear(Type ene) {
			weapon = weapLocation.GetComponentInChildren<Weapons>();
			if (weapon) {
				weapon.equip (this.unit, ene, 1);
			}
			
			Helmet helmetPiece = headLocation.GetComponentInChildren<Helmet>();
			// helmet = headLocation.GetComponentInChildren<Helmet>();
			if (helmetPiece) {
				helmetPiece.Equip (this.unit, 1);
				this.helmet = helmetPiece.GetComponent<Armor>();
			}
			
			Chest chestPiece = chestLocation.GetComponentInChildren<Chest>();
			if (chestPiece) {
				chestPiece.Equip (this.unit, 1);
				this.chest = chestPiece.GetComponent<Armor>();
			}
		}
	}
	
	public Inventory inventory;
	// might move to player depending on enemy stuff or have each class also have an inventory class inheriting this inventory
	[System.Serializable]
	public class Inventory {
		public int selected = 0;
		public bool keepItemActive = false;
		public Transform itemLocation;
		
		public List<Item> items = new List<Item>();
		
		private Character unit;
		
		public void SetUnit (Character unit) {
			this.unit = unit;
		}
		
		public void EquipItems(GameObject ability, Type ene) {
			GameObject itemObj = Instantiate(ability) as GameObject;
			itemObj.transform.SetParent(itemLocation, false);
			
			Item newItem = itemObj.GetComponent<Item>();
			if (newItem == null) newItem = itemObj.GetComponentInChildren<Item>();
			newItem.user = this.unit;
			newItem.opposition = ene;
			items.Add(newItem);
		}
	
		
		// Equip method for testing purposes
		public void equipItems(Type ene) {
			items.Clear ();
			items.AddRange(itemLocation.GetComponentsInChildren<Item>());
			
			if (items.Count == 0) {
				Debug.LogWarning(this.unit.gameObject.name + " does not have any abilities in the item slot.");
			}
			
			foreach (Item item in items) {
				item.user = this.unit;
				item.opposition = ene;
			}
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
		this.gear.SetUnit(this);
		this.inventory.SetUnit(this);
		this.animator = GetComponent<Animator>();
		this.rb = this.GetComponent<Rigidbody>();
		this.col = this.GetComponent<Collider>();

		facing = Vector3.forward;
		isDead = false;
		stunned = knockedback = animationLock = false;
		playSound = true;
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

	public virtual void SetGearAndAbilities() {
		gear.equipGear(opposition);
		inventory.equipItems(opposition);
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
		if (failure != null) StartCoroutine (makeSound (failure, playSound, failure.length));
	}

	public virtual IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position, 0.5f);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
	

}