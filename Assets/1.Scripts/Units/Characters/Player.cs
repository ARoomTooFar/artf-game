// Character class
// Base class that our heroes will inherit from

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Controls {
	//First 7 are Keys, last 2 are joystick axis
	public string up, down, left, right, attack, secItem, cycItem, hori, vert;
	//0 for Joystick off, 1 for Joystick on and no keys
	public int joyUsed;
}

[RequireComponent(typeof(Rigidbody))]
public class Player : Character, IMoveable {
	public bool inGrey;
	public int testDmg;
	public int greyDamage;
	public bool testable;
	public Transform weapLocation, headLocation, bodyLocation, itemLocation;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=10;
		stats.luck=0;
		inGrey = false;
		greyDamage = 0;
		testDmg = 0;
		testable = true;
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(stats.health <= 0){
			isDead = true;
		} else {
			isDead = false;
		}
		if(!isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
			
			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.nameHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd ;
			
			if (isGrounded) {
				actionCommands ();
				moveCommands ();
			} else {
				falling();
			}
			
			animationUpdate ();
		}
	}

	//-----------------------------------//
	// Movement interface implementation //
	//-----------------------------------//
	
	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;
		
		if (actable || (animator.GetBool("Charging") && (animSteHash == atkHashCharge || animSteHash == atkHashChgSwing))) {//gear.weapon.stats.curChgAtkTime > 0) { // Better Check here
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
			
			// facing = newMoveDir;
			if (newMoveDir != Vector3.zero) {
				newMoveDir.y = 0.0f;
				facing = newMoveDir;
			}
			
			rigidbody.velocity = newMoveDir.normalized * stats.speed * stats.spdManip.speedPercent;
		} else if (freeAnim){
			// Right now this stops momentum when performing an action
			// If we trash the rigidbody later, we won't need this
			rigidbody.velocity = Vector3.zero;
		}
		if(Input.GetKeyDown(KeyCode.Space)){
		    equipPiece("W0");
			equipPiece("C0");
			equipPiece("H1");
			equipPiece("I0");
			equipPiece("I1");
			equipPiece("I2");
		}
	}
	//Item type first char, number in array, second char++
	public virtual void equipPiece(System.String code){
		int index = (int)(code[1]-'0');
		if(code[0] == ('C')){
		   Quaternion butt = Quaternion.Euler(new Vector3(bodyLocation.eulerAngles.x+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.x,bodyLocation.eulerAngles.y+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.y,bodyLocation.eulerAngles.z+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.z));
		   gear.bodyArmor = (Armor) Instantiate(data.GetComponent<DataChest>().armory[index],headLocation.position,butt); //data.GetComponent<DataChest>().weaponry[1].transform.position+
		   gear.bodyArmor.transform.parent = bodyLocation.transform;
		   gear.bodyArmor.equip(gameObject.GetComponent<Character>());
		}else if(code[0] == ('H')){
		   //Quaternion butt = Quaternion.Euler(new Vector3(headLocation.eulerAngles.x+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.x,headLocation.eulerAngles.y+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.y,headLocation.eulerAngles.z+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.z));
		   gear.helmet = (Armor) Instantiate(data.GetComponent<DataChest>().armory[index],headLocation.position,transform.rotation); //data.GetComponent<DataChest>().weaponry[1].transform.position+
		   gear.helmet.transform.parent = headLocation.transform;
		   gear.helmet.equip(gameObject.GetComponent<Character>());
		}else if(code[0] == ('W')){
		   
		   Quaternion butt = Quaternion.Euler(new Vector3(weapLocation.eulerAngles.x+data.GetComponent<DataChest>().weaponry[index].transform.eulerAngles.x,weapLocation.eulerAngles.y+data.GetComponent<DataChest>().weaponry[index].transform.eulerAngles.y,weapLocation.eulerAngles.z+data.GetComponent<DataChest>().weaponry[index].transform.eulerAngles.z));
		   gear.weapon = (Weapons) Instantiate(data.GetComponent<DataChest>().weaponry[index],weapLocation.position,butt); //data.GetComponent<DataChest>().weaponry[1].transform.position+
		   gear.weapon.transform.parent = weapLocation.transform;
		   gear.weapon.equip(gameObject.GetComponent<Character>());
		}else if(code[0] == ('I')){
		   Quaternion butt = Quaternion.Euler(new Vector3(itemLocation.eulerAngles.x+data.GetComponent<DataChest>().inventory[index].transform.eulerAngles.x,itemLocation.eulerAngles.y+data.GetComponent<DataChest>().inventory[index].transform.eulerAngles.y,itemLocation.eulerAngles.z+data.GetComponent<DataChest>().inventory[index].transform.eulerAngles.z));
		   inventory.items.Add((Item) Instantiate(data.GetComponent<DataChest>().inventory[index],itemLocation.position,butt)); //data.GetComponent<DataChest>().weaponry[1].transform.position+
		   inventory.items[inventory.items.Count-1].transform.parent = itemLocation.transform;
		   inventory.equipItems(gameObject.GetComponent<Character>());
		}else{
			Debug.Log("You fucked up, bitch");
		}
	}
	//-------------------------------------//

	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Character striker) {
		if (!invincible) {
			dmgTaken = Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(facing, transform.position, striker.transform.position)), 1, 100000);
		
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			
			if (stats.health <= 0) {
				die();
			}
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible) {
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);

			if (stats.health <= 0) {
				die();
			}
		}
	}

	public override void die() {
		base.die();
	}

	//----------------------------------//

	// Grey Health functions
	public virtual int greyTest(int damage){
		if(((greyDamage + damage) > stats.health) && ((greyDamage + damage) < stats.maxHealth)){
			stats.health = 0;
			stats.isDead = true;
			return 0;
		}
		if(((greyDamage + damage) >= stats.maxHealth) && stats.health == stats.maxHealth){
			stats.health = 1;
			greyDamage = stats.maxHealth - 1;
			inGrey = true;
			return 0;
		}		
		if(damage > (stats.maxHealth/20)){
			//print("Got Here"+(stats.maxHealth/20)+":"+damage);
			int tempDmg = greyDamage;
			if(inGrey){
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+tempDmg);
				inGrey = true;
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				//print("True!WGAT:"+(stats.maxHealth/20 + tempDmg));
				return stats.health/20 + tempDmg;
			}else{
				inGrey = true;
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+(damage - stats.maxHealth/20));
				//print("True!NGAT:"+stats.maxHealth/20);
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				return stats.maxHealth/20;
			}
		}
		if(inGrey){
			inGrey = false;
			//print("True!WGBT:"+(damage + greyDamage));
			return damage + greyDamage;
		}
		else{
			inGrey = false;
			//print("True!NG:"+damage);
			return damage;
		}
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			testable = true;
			yield return 0;
		}
	}
	private IEnumerator RegenWait(){
		yield return new WaitForSeconds(1);
		if(inGrey && !stats.isDead){
			// print("Healed Grey and True");
			greyDamage--;
			if(greyDamage > 0){
				StartCoroutine("RegenWait");
			}
		}
		yield return 0;
	}

	//----------------------------------//
}