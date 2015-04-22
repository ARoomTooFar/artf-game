using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//initializes the 4 player panes
public class PlayerUI : MonoBehaviour {

	void Start () {

		//hardcoded just for the test scene
		PlayerUIPane playerPane = new PlayerUIPane();
		playerPane = transform.Find("Player1").gameObject.AddComponent<PlayerUIPane>();
		playerPane.initVals("PlayerZ");

		//for scenes with all 4 players
//		setUpPlayerUIPane("Player1");
//		setUpPlayerUIPane("Player2");
//		setUpPlayerUIPane("Player3");
//		setUpPlayerUIPane("Player4");

	}

	void setUpPlayerUIPane(string pl){
		PlayerUIPane playerPane = new PlayerUIPane();
		playerPane = transform.Find(pl).gameObject.AddComponent<PlayerUIPane>();
		playerPane.initVals(pl);
	}
}
