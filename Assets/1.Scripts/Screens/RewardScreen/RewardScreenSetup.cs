using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//sets up reward screen scripts for each player
public class RewardScreenSetup : MonoBehaviour {
	public Canvas rewardui;
	
	void Start () {
		rewardui = GameObject.Find("RewardScreenUI").GetComponent<Canvas>();

		PlayerRewardPanel p1panel = transform.Find("p1").gameObject.AddComponent<PlayerRewardPanel>();
		p1panel.setUpInputs("1","Q", "A", "Z"); //send joystick/button inputs

		PlayerRewardPanel p2panel = transform.Find("p2").gameObject.AddComponent<PlayerRewardPanel>();
		p2panel.setUpInputs("2", "W", "S", "X"); //send joystick/button inputs

		PlayerRewardPanel p3panel = transform.Find("p3").gameObject.AddComponent<PlayerRewardPanel>();
		p3panel.setUpInputs("3", "E", "D", "C"); //send joystick/button inputs

		PlayerRewardPanel p4panel = transform.Find("p4").gameObject.AddComponent<PlayerRewardPanel>();
		p4panel.setUpInputs("4", "R","F", "V"); //send joystick/button inputs


	}
}
