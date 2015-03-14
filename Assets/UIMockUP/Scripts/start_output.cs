using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class start_output : MonoBehaviour {

	[SerializeField] private Button transition_Button = null;
	// Use this for initialization
	void Start () {

		transition_Button.onClick.AddListener(() => {Transition();});

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Transition() {
		Application.LoadLevel ("playDemo");
	}
}
