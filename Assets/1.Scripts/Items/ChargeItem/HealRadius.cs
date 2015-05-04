using UnityEngine;
using System.Collections;

public class HealRadius : MonoBehaviour,IFallable {
	public float gravity = 2.0f;
	public bool isGrounded = false;
	public float decSpeed;
	public float baseSize;
	public bool used;
	public bool activated;
	private Healing buff;
	public ParticleSystem particles;
	public int duration;
	
	void Start () {
		decSpeed = .005f;
		baseSize = 8f;
		//transform.localScale = new Vector3(baseSize,.02f,baseSize);
		//StartCoroutine("pulse",pulseTime);
	}
	void Awake(){
		setInitValues(5);
		particles.enableEmission = false;
	}
	protected virtual void setInitValues(int num){
		duration = num;//chngNum(num);
		buff = new Healing(num);
	}
	protected virtual int chngNum(int num){
		return 5 - num;
	}
	// Update is called once per frame
	void Update () {
		isGrounded = Physics.Raycast (transform.position, -Vector3.up, 0.1f);
		if(isGrounded){
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			if(activated){
				transform.localScale -= new Vector3(decSpeed,0,decSpeed);
				if(transform.localScale.x < .5|| transform.localScale.z < .5){
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
	protected virtual void inHeal(Character ally) {
		if (ally && ally.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds)) {
			//ally.heal (1);
			ally.BDS.addBuffDebuff(buff, this.gameObject, 4.0f);
			StartCoroutine(healPulse(ally, 0.5f));
		}
	}
	protected IEnumerator healPulse(Character ally, float duration){
		while (duration > 0) {
			duration -= Time.deltaTime;
			yield return null;
		}
		inHeal(ally);
	}
	void OnTriggerEnter (Collider other) {
		//RiotShield rShield = other.GetComponent<RiotShield>();
		if (other.tag == "Grave") {
			//Debug.Log("RezConfirm");
			other.GetComponent<Grave>().ressurrection();
			used = true;
			//Destroy(gameObject);
		}
		Character ally = other.GetComponent<Character>();
		if (ally != null) {
			//decSpeed = decSpeed*2;
			ally.BDS.addBuffDebuff(buff, this.gameObject,duration);
		}
	}
	void OnTriggerExit (Collider other) {
		Character ally = other.GetComponent<Character>();
		if (ally != null) {
			//decSpeed = decSpeed/2;
			//ally.BDS.rmvBuffDebuff(buff, this.gameObject);
		}
	}
}
