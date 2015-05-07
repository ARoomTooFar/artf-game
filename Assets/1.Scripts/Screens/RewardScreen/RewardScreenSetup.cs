using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//sets up reward screen scripts for each player
public class RewardScreenSetup : MonoBehaviour {
	public Canvas rewardui;
	
	void Start () {
		rewardui = GameObject.Find("RewardScreenUI").GetComponent<Canvas>();

		transform.Find("p1").gameObject.AddComponent<PlayerRewardPanel>();
		//		transform.Find("p2").gameObject.AddComponent<PlayerRewardPanel>();
		//		transform.Find("p3").gameObject.AddComponent<PlayerRewardPanel>();
		//		transform.Find("p4").gameObject.AddComponent<PlayerRewardPanel>();


	}
}
