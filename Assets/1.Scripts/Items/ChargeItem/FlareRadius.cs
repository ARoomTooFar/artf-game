using UnityEngine;
using System.Collections;

public class FlareRadius : MonoBehaviour,IFallable {
	public float gravity = 1.0f;
	public bool isGrounded = false;
	public float decSpeed;
	public float baseSize;
	public bool used;
	public bool activated;
	public ParticleSystem particles;
	public float duration;
	public GameObject dummy;
	
	void Start () {
		decSpeed = .0225f;
		baseSize = 64f;
		//transform.localScale = new Vector3(baseSize,.02f,baseSize);
		//StartCoroutine("pulse",pulseTime);
	}
	void Awake(){
		setInitValues(4);
		particles.enableEmission = false;
	}
	protected virtual void setInitValues(int num){
		duration = num;
	}
	// Update is called once per frame
	void Update () {
		isGrounded = Physics.Raycast (transform.position, -Vector3.up, 0.2f);
		if(isGrounded){
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			if(activated){
				transform.localScale -= new Vector3(decSpeed,0,decSpeed);
				if(transform.localScale.x < .5|| transform.localScale.z < .5){
					Prop[] flareNode = GetComponentsInChildren<Prop>();
					if(flareNode[0] != null){
						flareNode[0].die();
					}
					Destroy(gameObject);
				}
			}else{
				startSize();
			}
		}else{
			if(!activated){
				falling();
			}
		}/*else if(isGrounded && !activated){
			startSize();
		}*/
	}
	protected virtual void startSize(){
		activated = true;
		particles.enableEmission = true;
		transform.localScale = new Vector3(baseSize,.02f,baseSize);
	}
	//----------------------------------//
	// Falling Interface Implementation //
	//----------------------------------//

	public virtual void falling() {
		// fake gravity
		// Animation make it so rigidbody gravity works oddly due to some gravity weight
		// Seems like Unity Pro is needed to change that, so unless we get it, this will suffice 
		GetComponent<Rigidbody>().velocity = new Vector3 (0.0f, -gravity, 0.0f);
	}

	//----------------------------------//
	/*protected virtual void inNet(Character enemy) {
		if (enemy && enemy.collider.bounds.Intersects(collider.bounds)) {
			enemy.BDS.addBuffDebuff(debuff, this.gameObject, 4.0f);
			StartCoroutine(netPulse(enemy, 0.75f));
		}
	}
	protected IEnumerator netPulse(Character enemy, float duration){
		while (duration > 0) {
			duration -= Time.deltaTime;
			yield return null;
		}
		//inNet(enemy);
	}*/
	void OnTriggerEnter (Collider other) {
		//RiotShield rShield = other.GetComponent<RiotShield>();
		/*if (other.tag == "Grave") {
			//Debug.Log("RezConfirm");
			other.GetComponent<Grave>().ressurrection();
			used = true;
			//Destroy(gameObject);
		}*/
		NewEnemy foe = other.GetComponent<NewEnemy>();
		Enemy enemy = other.GetComponent<Enemy>();
		if (enemy != null) {
			decSpeed = decSpeed*4;
			enemy.taunted(dummy);
		}
		if (foe != null) {
			decSpeed = decSpeed*4;
			foe.taunted(dummy);
		}
		
	}
	void OnTriggerExit (Collider other) {
		Enemy enemy = other.GetComponent<Enemy>();
		NewEnemy foe = other.GetComponent<NewEnemy>();
		if (enemy != null || foe != null) {
			decSpeed = decSpeed/4;
			//enemy.BDS.timedRemoval(debuff, this.gameObject,duration);
		}
	}
}