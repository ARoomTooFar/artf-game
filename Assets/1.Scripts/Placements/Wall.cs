using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public bool show;
	public float disappear;
	public GameObject stand;
	protected virtual void Start(){
		show = true;
		disappear = 3f;
	}
	protected virtual void Awake () {
	}

	public virtual void toggleShow(){
		if(show){
			show = false;
			if(stand !=null){
				stand.GetComponent<Renderer>().enabled = true;
			}
			GetComponent<Renderer>().enabled = false;
			// GetComponent<Collider>().enabled = false;
			StopCoroutine("revWait");
			StartCoroutine("revWait",disappear);
		}
	}

	protected virtual void Update(){
	
	}
	private IEnumerator revWait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			//testable = true;
			if(timer > duration -1f){
				show = true;
				GetComponent<Collider>().enabled = true;
			}
			yield return 0;
		}
		show = true;
		if(stand !=null){
			stand.GetComponent<Renderer>().enabled = false;
		}
		//GetComponent<Collider>().enabled = true;
		GetComponent<Renderer>().enabled = true;
	}
}
