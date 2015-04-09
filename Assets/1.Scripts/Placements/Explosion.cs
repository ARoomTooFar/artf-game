using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float lifetime;
	public bool charDeath;
	public GameObject grave;
	public Character unit;
	protected Quaternion spray;
	protected float variance;
	//protected 
	// Use this for initialization
	void Start () {
		variance = 45f;
		spray = Quaternion.Euler(new Vector3(unit.transform.eulerAngles.x,Random.Range(-variance+unit.transform.eulerAngles.y,variance+unit.transform.eulerAngles.y),unit.transform.eulerAngles.z));
		lifetime = 2f;
		StartCoroutine(Wait(lifetime-.25f));
	}
	public void setInitValues(Character p, bool state, GameObject drop){
		unit = p;
		charDeath = state;
	}
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, lifetime);
	}
	public void lootBoom(){
		if(unit.drop !=null){
			for(int i = 0; i<4; i++){//Four is replaceable
			}
		}
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		if(charDeath){
			if(unit.GetType() == typeof(Player)){
				Grave g = ((GameObject)Instantiate(grave, unit.transform.position, grave.transform.rotation)).GetComponent<Grave>();
				g.setInitValues((Player)unit);
			}
			//Instantiate(grave,transform.position+grave.transform.position,transform.rotation);
		}
	}
}
