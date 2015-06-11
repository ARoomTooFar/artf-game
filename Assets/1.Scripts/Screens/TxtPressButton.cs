using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TxtPressButton : MonoBehaviour {
	private Text txt;
	private Color txtColor;
	private bool reverse;
	
	void Start () {
		txt = gameObject.GetComponent<Text> ();
		txtColor = txt.color;
	}

	void Update () {
		if (txtColor.a <= 0) {
			reverse = true;
		} if (txtColor.a >= 1) {
			reverse = false;
		}

		if (reverse) {
			txtColor.a += 0.01f;
		} else {
			txtColor.a -= 0.01f;
		}

		txt.color = txtColor;
	}
}
