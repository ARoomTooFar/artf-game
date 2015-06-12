using UnityEngine;
using System.Collections;

public class NetRadius : MonoBehaviour {
	public float gravity = 1.0f;
	public bool isGrounded = false;
	public float decSpeed;
	public float baseSize;
	public bool used;
	public bool activated;
	private Immobilize debuff;
	public ParticleSystem particles;
	public float duration;
	
	void Start () {
		decSpeed = .025f;
		baseSize = 8f;
		//transform.localScale = new Vector3(baseSize,.02f,baseSize);
		//StartCoroutine("pulse",pulseTime);
		Invoke("DestroySelf", 5f);
	}
	
	void Awake(){
		setInitValues(3);
		// particles.enableEmission = false;
	}
	protected virtual void setInitValues(int num){
		duration = num;
		debuff = new Immobilize();
	}
	// Update is called once per frame
	void Update () {

	}

	private void DestroySelf() {
		Destroy (this.gameObject);
	}

	void OnTriggerEnter (Collider other) {
		Character enemy = other.GetComponent<Character>();
		if (enemy != null) {
			//decSpeed = decSpeed*4;
			enemy.BDS.addBuffDebuff(debuff, this.gameObject,duration);
		}
	}
	void OnTriggerExit (Collider other) {
	}
}
