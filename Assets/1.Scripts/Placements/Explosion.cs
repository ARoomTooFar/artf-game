using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float lifetime;
	public bool charDeath;
	public GameObject grave;
	public LootDrop lootDrop;
	public Character unit;
	protected Quaternion spray;
	protected float variance;
	protected Transform localTarget,savedSpot;
	//protected 
	// Use this for initialization
	void Start () {
		variance = 45f;
		localTarget = this.transform;
		localTarget.position = new Vector3(localTarget.position.x,localTarget.position.y+5,localTarget.position.z);
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
	/*public void lootBoom(){
		if(unit.drop !=null){
			for(int i = 0; i<4; i++){//Four is replaceable
				localTarget.position = savedSpot.position;
				spray = Quaternion.Euler(new Vector3(localTarget.transform.eulerAngles.x,-variance*2+localTarget.transform.eulerAngles.y,localTarget.transform.eulerAngles.z));
				localTarget.rotation = spray;//
				localTarget.position = Vector3.MoveTowards(localTarget.position, localTarget.position+localTarget.transform.forward*8, 10);
				lootDrop = ((GameObject)Instantiate(unit.drop, unit.transform.position, unit.transform.rotation)).GetComponent<LootDrop>();
				lootDrop.shoot (lootDrop.transform.AngledArcTrajectory(localTarget.transform.position,10));
				spray = Quaternion.Euler(new Vector3(localTarget.transform.eulerAngles.x,-variance*2+localTarget.transform.eulerAngles.y,localTarget.transform.eulerAngles.z));
				//localTarget.rotation = spray;//
			}
		}
	}*/
	
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		if(charDeath){
			if(unit.GetType() == typeof(Player)){
				Grave g = ((GameObject)Instantiate(grave, this.transform.position, this.transform.rotation)).GetComponent<Grave>();
				g.setInitValues((Player)unit);
				//lootBoom();
			}
			//Instantiate(grave,transform.position+grave.transform.position,transform.rotation);
		}
	}

}
