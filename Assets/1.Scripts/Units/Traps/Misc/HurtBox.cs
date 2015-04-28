using UnityEngine;
using System.Collections;

public class HurtBox : MonoBehaviour {

	public int damage;

	// Use this for initialization
	protected virtual void Start () {
	}
	
	protected virtual void setInitValues() {
	}
	
	protected virtual void FixedUpdate() {
	}
	
	// Update is called once per framea
	protected virtual void Update () {
	}

	void OnTriggerEnter(Collider other) {
		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(damage);
		}
	}
}
