using UnityEngine;
using System.Collections;

public class HealRadius : MonoBehaviour {
	public float decSpeed;
	public float baseSize;
	public bool used;
	public float pulseTime;
	public bool healing;
	//public Player
	// Use this for initialization
	void Start () {
		healing = true;
		decSpeed = .0025f;
		baseSize = 8f;
		pulseTime = .75f;
		transform.localScale = new Vector3(baseSize,.02f,baseSize);
		//StartCoroutine("pulse",pulseTime);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale -= new Vector3(decSpeed,0,decSpeed);
		if(transform.localScale.x < .5|| transform.localScale.z < .5){
			Destroy(gameObject);
		}
	}
	private IEnumerator pulse(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		healing = !healing;
		//StopCoroutine("pulse");
		//StartCoroutine("pulse",duration);
	}
	void OnTriggerStay (Collider other) {
		//RiotShield rShield = other.GetComponent<RiotShield>();
		if (other.tag == "Grave") {
			//Debug.Log("RezConfirm");
			other.GetComponent<Grave>().ressurrection();
			used = true;
			//Destroy(gameObject);
		}
		if(other.tag == "Player"){
			if(healing){
				other.GetComponent<Player>().heal(1);
				healing = false;
				StopCoroutine("pulse");
				StartCoroutine("pulse",pulseTime);
			}
		}
	}
	void OnTriggerEnter (Collider other) {
		//RiotShield rShield = other.GetComponent<RiotShield>();
		if (other.tag == "Grave") {
			//Debug.Log("RezConfirm");
			other.GetComponent<Grave>().ressurrection();
			used = true;
			//Destroy(gameObject);
		}
		if(other.tag == "Player"){
			if(healing){
				other.GetComponent<Player>().heal(1);
				healing = false;
				StopCoroutine("pulse");
				StartCoroutine("pulse",pulseTime);
			}
		}
	}
}
