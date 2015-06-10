using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Rewired;

//sets up reward screen scripts for each player
public class RewardScreenSetup : MonoBehaviour {
	public Canvas rewardui;
//	public Controls controlsp1;
//	public Controls controlsp2;
//	public Controls controlsp3;
//	public Controls controlsp4;
	private Rewired.Player cont1;
	private Rewired.Player cont2;
	private Rewired.Player cont3;
	private Rewired.Player cont4;
	public Text txtCountdown;

	private GSManager gsManager;
	private float startTime;
	private int countDownVal = 30;
	private int secondsPassed = 1;
	
	void Start () {
		gsManager = GameObject.Find("/GSManager").GetComponent<GSManager>();
		rewardui = GameObject.Find("RewardScreenUI").GetComponent<Canvas>();
		
		PlayerRewardPanel p1panel = transform.Find("p1").gameObject.AddComponent<PlayerRewardPanel>();
		cont1 = ReInput.players.GetPlayer (2);
		p1panel.setUpInputs("T","Q", "A", "Z", cont1); //send joystick/button inputs

		PlayerRewardPanel p2panel = transform.Find("p2").gameObject.AddComponent<PlayerRewardPanel>();
		cont2 = ReInput.players.GetPlayer (3);
		p2panel.setUpInputs("Y", "W", "S", "X", cont2); //send joystick/button inputs

		PlayerRewardPanel p3panel = transform.Find("p3").gameObject.AddComponent<PlayerRewardPanel>();
		cont3 = ReInput.players.GetPlayer (0);
		p3panel.setUpInputs("U", "E", "D", "C", cont3); //send joystick/button inputs

		PlayerRewardPanel p4panel = transform.Find("p4").gameObject.AddComponent<PlayerRewardPanel>();
		cont4 = ReInput.players.GetPlayer (1);
		p4panel.setUpInputs("I", "R","F", "V", cont4); //send joystick/button inputs

		// start timer
		startTime = Time.time;
	}

	void Update () {
		// decrement countdown
		if (Time.time > startTime + secondsPassed) {
			txtCountdown.text = Convert.ToString (countDownVal - secondsPassed);
			++secondsPassed;
		}

		// change scenes when timer reaches 0
		if (secondsPassed >= 31) {
			gsManager.LoadScene ("MainMenu");
		}
	}
}
