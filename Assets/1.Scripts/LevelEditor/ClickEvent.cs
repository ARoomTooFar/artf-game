using UnityEngine;
using System;
using System.Collections;

public class ClickEvent : MonoBehaviour {

	public virtual IEnumerator onClick(Vector3 initPosition){
		Debug.Log (this.gameObject.name);
		yield return null;
	}

}
