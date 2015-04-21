using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {
	public Slider healthBar;
	public Canvas healthBarCanvas;
	public float health = .5f;

	void Start () {
		healthBar = this.gameObject.GetComponentInChildren<Slider>();
		healthBarCanvas = this.gameObject.GetComponent("Canvas") as Canvas;
	}
	

	void Update () {
		healthBar.value = health;
		if(health == 1){
			//Something for showing the health bar, doesn't need to be up if no damage is done. 
		}
		if(health == 0){
			//Something for disappearing the health bar
		}
	}

	//Update causes itemObjectUI flickering. LateUpdate prevents it
	void LateUpdate(){
		faceUIToCamera();
	}

	void faceUIToCamera(){
		Vector3 p = new Vector3();

		p = Camera.main.transform.rotation.eulerAngles;
		healthBarCanvas.transform.rotation = Quaternion.Euler(p);
	}
}
