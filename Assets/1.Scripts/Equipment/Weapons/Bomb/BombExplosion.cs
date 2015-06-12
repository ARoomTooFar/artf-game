using UnityEngine;
using System.Collections;

public class BombExplosion : MonoBehaviour {
	//-------------------//
	// Primary Functions //
	//-------------------//
	
	public AudioClip explosionSound;

	void Start () {
		if (this.explosionSound != null) {
			AudioSource.PlayClipAtPoint (explosionSound, transform.position);
		}
		Destroy (gameObject, 1.0f);
	}

	// Update is called once per frame
	void Update () {

	}

	//--------------------//


	//------------------//
	// Public Functions //
	//------------------//
	

	//--------------------//
}