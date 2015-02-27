using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public NavMeshObstacle obs;
	public bool show;
	public float disappear;
	public GameObject stand;
	protected virtual void Start(){
		show = true;
		disappear = 3f;
	}
	protected virtual void Awake () {
		obs = GetComponent<NavMeshObstacle> ();
		obs.carving = true;
	}
	public virtual void toggleShow(){
		if(show){
		show = false;
		stand.renderer.enabled = true;
		renderer.enabled = false;
		// collider.enabled = false;
		//StopCoroutine("revWait");
		StartCoroutine("revWait",disappear);
		}
	}
	protected virtual void Update(){
	
	}
	private IEnumerator revWait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			//testable = true;
			yield return 0;
		}
		show = true;
		stand.renderer.enabled = false;
		// collider.enabled = true;
		renderer.enabled = true;
	}
}
