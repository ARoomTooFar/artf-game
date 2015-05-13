using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//sets up reward screen scripts for each player
public class RewardScreenSetup : MonoBehaviour {
	public Canvas rewardui;
	public Controls controls;
	
	void Start () {
		rewardui = GameObject.Find("RewardScreenUI").GetComponent<Canvas>();


		PlayerRewardPanel p1panel = transform.Find("p1").gameObject.AddComponent<PlayerRewardPanel>();
		p1panel.setUpInputs("T","Q", "A", "Z", controls); //send joystick/button inputs

		PlayerRewardPanel p2panel = transform.Find("p2").gameObject.AddComponent<PlayerRewardPanel>();
		p2panel.setUpInputs("Y", "W", "S", "X", controls); //send joystick/button inputs

		PlayerRewardPanel p3panel = transform.Find("p3").gameObject.AddComponent<PlayerRewardPanel>();
		p3panel.setUpInputs("U", "E", "D", "C", controls); //send joystick/button inputs

		PlayerRewardPanel p4panel = transform.Find("p4").gameObject.AddComponent<PlayerRewardPanel>();
		p4panel.setUpInputs("I", "R","F", "V", controls); //send joystick/button inputs


	}
}
