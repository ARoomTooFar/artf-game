using UnityEngine;
using System.Collections;

public class HookHelper : MonoBehaviour {
	public Hook spawner;
	void OnTriggerEnter (Collider other) {
		if(!spawner.hit){
			if (other.tag == "Wall") {
				spawner.hit = true;
				spawner.check = true;
			}
			IForcible<Vector3,float> component = (IForcible<Vector3,float>) other.GetComponent( typeof(IForcible<Vector3,float>) );
			spawner.foe = other.GetComponent<Character>();
			if( component != null && spawner.foe != null) {
				spawner.hit = true;
				//spawner.check = true;
				GetComponent<Collider>().enabled = false;
			}
		}
		if (other.gameObject == spawner.gameObject && spawner.check) {
			// this.transform = spawner.transform;

		}
	}
}
