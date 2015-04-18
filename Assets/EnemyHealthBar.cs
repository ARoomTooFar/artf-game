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
