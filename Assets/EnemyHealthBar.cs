using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {
	Slider healthBar;
	Canvas healthBarCanvas;
	float health = 0.7f;

	void Start () {
		healthBar = this.transform.Find("HealthBar").gameObject.GetComponent("Slider") as Slider;
		healthBarCanvas = this.gameObject.GetComponent("Canvas") as Canvas;
	}
	

	void Update () {
		healthBar.value = health;
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
