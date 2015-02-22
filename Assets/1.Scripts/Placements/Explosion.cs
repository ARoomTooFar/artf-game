using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float lifetime;
	public bool charDeath;
	public GameObject grave;
	public Player unit;
	// Use this for initialization
	void Start () {
		lifetime = 2f;
		StartCoroutine(Wait(lifetime-.25f));
	}
	public void setInitValues(Player p, bool state){
		unit = p;
		charDeath = state;
	}
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, lifetime);
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		if(charDeath){
			Instantiate(grave,transform.position+grave.transform.position,transform.rotation);
		}
	}
}
