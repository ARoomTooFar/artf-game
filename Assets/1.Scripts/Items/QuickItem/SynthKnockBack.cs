using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SynthKnockBack : QuickItem {

	public List<Character> enemies;

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		cooldown = 10.0f;
	}

	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		this.knock ();
		this.animDone ();  
	}
	
	protected override void animDone() {
		base.animDone();
	}

	protected void knock(){
		foreach (Character ene in enemies) {
			Vector3 newpos = (user.rb.transform.position - ene.rb.transform.position) * (-1);
			newpos.Normalize();
			ene.transform.position = new Vector3(ene.transform.position.x + (newpos.x * 10), ene.transform.position.y, ene.transform.position.z + (newpos.z * 10));
		}
	}

	void OnTriggerEnter (Collider other) {
		
		// Will need a differentiation in the future(Or not if we want this)
		//     I suggest having the users know what is there enemy and settign ti that way somehow
		Character enemy = other.GetComponent<Character>();
		IForcible<Vector3, float> component = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>));
		if(component != null && enemy != null) {
			enemies.Add (enemy);
		}
	}

	void OnTriggerExit (Collider other){
		Character enemy = other.GetComponent<Character>();
		IForcible<Vector3, float> component = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>));
		if(component != null && enemy != null) {
			enemies.Remove (enemy);
		}
	}
}
