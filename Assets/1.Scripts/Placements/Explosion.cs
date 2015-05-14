using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float lifetime;
	public bool charDeath;
	public GameObject grave;
	public GameObject lootDrop;
	public Character unit;
	protected Quaternion spray;
	protected float variance;
	protected Transform localTarget,savedSpot;
	//protected 
	// Use this for initialization
	void Start () {
		lootDrop = unit.drop;
		variance = 45f;
		localTarget = this.transform;
		//localTarget.position = new Vector3(localTarget.position.x,localTarget.position.y,localTarget.position.z);
		savedSpot = localTarget;
		if(unit!=null){
			spray = Quaternion.Euler(new Vector3(unit.transform.eulerAngles.x,Random.Range(-variance+unit.transform.eulerAngles.y,variance+unit.transform.eulerAngles.y),unit.transform.eulerAngles.z));
		}
		lifetime = 2f;
		StartCoroutine(Wait(lifetime-.25f));
	}
	public void setInitValues(Character p, bool state){
		unit = p;
		charDeath = state;
	}
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, lifetime);
	}
	public void lootBoom(){
		if(lootDrop !=null){
			LootDrop loot = ((GameObject)Instantiate (lootDrop, this.transform.position, this.transform.rotation)).GetComponent<LootDrop>();
			loot.starter = transform;
			loot.starter.Translate (Vector3.forward *15);
		}
	}
	
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		if(charDeath){
			if(unit.GetType() == typeof(Player)){
				Grave g = ((GameObject)Instantiate(grave, this.transform.position, this.transform.rotation)).GetComponent<Grave>();
				g.setInitValues((Player)unit);
				//lootBoom();
			}else{
				lootBoom ();
			}
			//Instantiate(grave,transform.position+grave.transform.position,transform.rotation);
		}
	}

}
