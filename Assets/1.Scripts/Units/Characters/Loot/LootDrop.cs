using UnityEngine;
using System.Collections;
using System;

public class LootDrop : MonoBehaviour {
	public float value,high,low;
	public int rating;
	public LootSystem lootage;
	public GameObject target;
	// Use this for initialization
	void Start () {
		lootage = (LootSystem) FindObjectOfType(typeof(LootSystem));
		/*if(lootage){//Used as a debug check for looting~
			Debug.Log("LootSystem");
		}else{
			Debug.Log("NoLootSystem");
		}*/
		setInitValues(0);//To be removed soon(Just for base test)
	}
	
	// Update is called once per frame
	void Update () {
		if(target!=null){
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, .2f);
			if(transform.position == target.transform.position){
				if(lootage){
					lootage.addRating(value);
					Destroy(gameObject);
				}
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
			//Debug.Log("Looted, bitch");//Get the loot, homie
			if(other.GetComponent<Player>().luckCheck()){
				value += value/2; //Luck buff on drops :D
			}
			GetComponent<Collider>().enabled = false;//Turn of collider so loot doesn't intercept to another unit. 
		}
	}
	public virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}
}
