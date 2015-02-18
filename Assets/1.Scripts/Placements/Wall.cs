using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public NavMeshObstacle obs;
	public bool show;
	public float disappear;
	void Start(){
		show = true;
		disappear = 1f;
	}
	void Awake () {
		obs = GetComponent<NavMeshObstacle> ();
		obs.carving = true;
	}
	public void toggleShow(){
		show = false;
		renderer.enabled = false;
		StopCoroutine("Wait");
		StartCoroutine("Wait",disappear);
	}
	void Update(){
	
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			//testable = true;
			yield return 0;
		}
		show = true;
		renderer.enabled = true;
	}
}
