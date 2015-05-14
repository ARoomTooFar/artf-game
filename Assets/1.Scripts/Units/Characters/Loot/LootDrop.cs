using UnityEngine;
using System.Collections;
using System;

public class LootDrop : MonoBehaviour {
	public float value,high,low,growth,transition, gx, gz, dampening,distance;
    public Transform starter;
	public int rating,direction;
	public LootSystem lootage;
	public GameObject target;
	public bool move,stall;
	// Use this for initialization
	void Start () {
        starter = transform;
		lootage = (LootSystem) FindObjectOfType(typeof(LootSystem));
        direction = 0;
        growth = .01f;
		dampening = growth * 15;
		distance = 10;
		move = true;
		setInitValues(0);//To be removed soon(Just for base test)
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, .2f);
			if (transform.position == target.transform.position) {
				if (lootage) {
					lootage.addRating (value);
					Destroy (gameObject);
				}
			}
		} else if (move && target == null && !stall) {
			if (direction == 0) {
				transition += growth;
				gx = (dampening - transition);
				gz = transition;
				transform.position = new Vector3 (transform.position.x + gx, transform.position.y, transform.position.z + gz);
			} else if (direction == 1) {
				transition += growth;
				gx = transition;
				gz = (dampening - transition);
				transform.position = new Vector3 (transform.position.x - gx, transform.position.y, transform.position.z + gz);
			}
            else if (direction == 2) {
				transition += growth;
				gx = (dampening - transition);
				gz = transition;
				transform.position = new Vector3 (transform.position.x - gx, transform.position.y, transform.position.z - gz);
			} else if (direction == 3) {
				transition += growth;
				gx = transition;
				gz = (dampening - transition);
				transform.position = new Vector3 (transform.position.x + gx, transform.position.y, transform.position.z - gz);
			} else {
				transition = 0;
			}
			if (transition > dampening) {
				transition = 0;
				direction++;
				//print (dampening);
				if (direction == 4) {
					//dampening--;
					if (dampening <= 0) {
						move = false;
						Destroy (gameObject, 1);
					} else {
						dampening -= growth;
					}
					direction = 0;
				}
			}
		} else if (!move && target == null && !stall) {
			if(starter != null){
				transform.position = Vector3.MoveTowards (transform.position, starter.position, .2f);
				distance = Vector3.Distance (starter.position,transform.position);
				if(distance < .5f){
					move = true;
				}
			}else{
				move = true;
			}
		}
	}
	protected virtual void setInitValues(int rating){//0-10
		high = .5f + .25f*rating;
		if(high - .5f < 0){
			low = 0;//Don't want to be negative on the drop, y'know~?
		}else{
			low = high - .5f;
		}
		castValue();
	}
	protected virtual void castValue(){
		value = UnityEngine.Random.Range(low,high);
	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player"){
			target = other.gameObject;
			GetComponent<Collider>().enabled = false;//Turn of collider so loot doesn't intercept to another unit. 
		}
		if ((other.tag == "Wall" || other.tag == "Door") && target == null) {
			stall = true;
		}

	}
	public virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}
}
