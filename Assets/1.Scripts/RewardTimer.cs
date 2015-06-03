using UnityEngine;
using System.Collections;

public class RewardTimer : MonoBehaviour {

	private GSManager gsm;
	public int num; //the number that is incrimented

	// Use this for initialization
	void Start () {
		gsm = GameObject.Find("GSManager").GetComponent<GSManager>();
	}
	
	// Update is called once per frame
	void Update () {

		if (num == 0) {
			gsm.LoadScene ("MainMenu");
		}

	}

	//adds 1 to the current number
	void subNum() {
		num ++;
	}
}
