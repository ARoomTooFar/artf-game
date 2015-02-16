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
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(damage);
		}
	}
}
