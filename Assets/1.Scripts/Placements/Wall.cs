using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public bool show;
	public float disappear;
	public float timeElapsed;
	public GameObject stand;

	protected virtual void Start() {
		show = true;
		disappear = .01f;
		timeElapsed = 4;
	}

	protected virtual void Awake() {
	}

	public virtual void toggleShow() {
		if(this.gameObject.transform.Find("Wall") != null){
			GameObject wall = this.gameObject.transform.Find("Wall").gameObject;

			foreach(Material mat in wall.GetComponent<Renderer>().materials) {
				if(mat.HasProperty("_Color")) {
					Color trans = mat.color;
					trans.a = .2f;
					mat.color = trans;
				}
			}
		}

		timeElapsed = 0;
	}

	protected virtual void Update() {
		if(timeElapsed < disappear) {
			timeElapsed += Time.deltaTime;
		} else {
			if(this.gameObject.transform.Find("Wall") != null){
				GameObject wall = this.gameObject.transform.Find("Wall").gameObject;

				foreach(Material mat in wall.GetComponent<Renderer>().materials) {
					if(mat.HasProperty("_Color")) {
						Color trans = mat.color;
						trans.a = 1f;
						mat.color = trans;
					}
				}
			}
		}
	}
	
}
