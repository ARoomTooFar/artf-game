using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RewardScreenSetup : MonoBehaviour {
	public Canvas rewardui;
	
	void Start () {
		rewardui = GameObject.Find("RewardScreenUI").GetComponent<Canvas>();

		transform.Find("p1").gameObject.AddComponent<PlayerRewardScreen>();
//		transform.Find("p2").gameObject.AddComponent<PlayerRewardScreen>();
//		transform.Find("p3").gameObject.AddComponent<PlayerRewardScreen>();
//		transform.Find("p4").gameObject.AddComponent<PlayerRewardScreen>();


	}
}
