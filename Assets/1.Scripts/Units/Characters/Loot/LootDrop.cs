using UnityEngine;
using System.Collections;
using System;

public class LootDrop : MonoBehaviour {
	public float value,high,low,growth,transition, gx, gz;
    public Transform starter;
	public int rating,direction;
	public LootSystem lootage;
	public GameObject target;
	// Use this for initialization
	void Start () {
        starter = transform;
		lootage = (LootSystem) FindObjectOfType(typeof(LootSystem));
		/*if(lootage){//Used as a debug check for looting~
			Debug.Log("LootSystem");
		}else{
			Debug.Log("NoLootSystem");
		}*/
        direction = 0;
        growth = .005f;
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
        else
        {
            if (direction == 0)
            {
                transition += growth;
                gx = transition;
                gz = transition;
                transform.position = new Vector3(transform.position.x + transition, transform.position.y, transform.position.z + transition);
            }
            else if (direction == 1)
            {
                transition += growth;
                transform.position = new Vector3(transform.position.x - transition, transform.position.y, transform.position.z + transition);
                //transform.position.x -= transition;
                // transform.position.z += transition;
            }//
            else if (direction == 2)
            {
                transition += growth;
                transform.position = new Vector3(transform.position.x - transition, transform.position.y, transform.position.z - transition);
                //transform.position.x -= transition;
                //transform.position.z += transition;
            }
            else if (direction == 3)
            {
                transition += growth;
                transform.position = new Vector3(transform.position.x + transition, transform.position.y, transform.position.z - transition);
                //transform.position.x -= transition;
                //transform.position.z -= transition;
            }
            else{
                transition = 0;
            }
            if (transition > growth*30)
            {
                transition = 0;
                direction++;
                if (direction == 4)
                {
                    direction = 0;
                }
            }
        }
	}
    /*protected virtual void spiralOut()
    {
        int direction = 0;//Should be 1-4
        while(direction < 4){
            
        }
    }*/
    protected virtual void moveHere()
    {
        while (transition < 5 && direction < 5)
        {
            if (direction == 0)
            {
                transition += 0.01f;
                transform.position = new Vector3(transform.position.x+transition,transform.position.y,transform.position.z+transition);
            }
            else if (direction == 1)
            {
                transition += 0.01f;
                transform.position = new Vector3(transform.position.x - transition, transform.position.y, transform.position.z + transition);
                //transform.position.x -= transition;
               // transform.position.z += transition;
            }//
            else if (direction == 2)
            {
                transition += 0.01f;
                transform.position = new Vector3(transform.position.x - transition, transform.position.y, transform.position.z - transition);
                //transform.position.x -= transition;
                //transform.position.z += transition;
            }
            else if (direction == 3)
            {
                transition += 0.01f;
                transform.position = new Vector3(transform.position.x + transition, transform.position.y, transform.position.z - transition);
                //transform.position.x -= transition;
                //transform.position.z -= transition;
            }
            else//larger than 4
            {
                
            }
        }
        if(transition >= 5){
            direction++;
            transition = 0;
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

			/*
			if(other.GetComponent<Player>().luckCheck()){
				value += value/2; //Luck buff on drops :D
			}*/
			GetComponent<Collider>().enabled = false;//Turn of collider so loot doesn't intercept to another unit. 
		}
	}
	public virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}
}
