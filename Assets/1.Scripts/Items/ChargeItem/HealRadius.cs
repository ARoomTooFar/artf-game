using UnityEngine;
using System.Collections;

public class HealRadius : MonoBehaviour {
	public float decSpeed;
	public float baseSize;
	public bool used;
	private Healing buff;
	//public Player
	
	void Start () {
		decSpeed = .0025f;
		baseSize = 8f;
		transform.localScale = new Vector3(baseSize,.02f,baseSize);
		//StartCoroutine("pulse",pulseTime);
	}
	void Awake(){
		setInitValues();
	}
	protected virtual void setInitValues(){
		buff = new Healing(2);
	}
	// Update is called once per frame
	void Update () {
		transform.localScale -= new Vector3(decSpeed,0,decSpeed);
		if(transform.localScale.x < .5|| transform.localScale.z < .5){
			Destroy(gameObject);
		}
	}
	protected virtual void inHeal(Character ally) {
		if (ally && ally.collider.bounds.Intersects(collider.bounds)) {
			ally.heal (1);
			ally.BDS.addBuffDebuff(buff, this.gameObject, 4.0f);
			StartCoroutine(healPulse(ally, 0.75f));
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
			ally.BDS.addBuffDebuff(buff, this.gameObject);
		}
	}
	void OnTriggerExit (Collider other) {
		Character ally = other.GetComponent<Character>();
		if (ally != null) {
			ally.BDS.rmvBuffDebuff(buff, this.gameObject);
		}
	}
}
