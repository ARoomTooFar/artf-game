using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//initializes the 4 player panes
public class PlayerUI : MonoBehaviour {

	void Start () {

		//hardcoded just for the test scene
		//PlayerUIPane playerPane = new PlayerUIPane();
		//playerPane = transform.Find("Player1").gameObject.AddComponent<PlayerUIPane>();

		//for scenes with all 4 players

		setUpPlayerUIPane("Player1");
		setUpPlayerUIPane("Player2");
		setUpPlayerUIPane("Player3");
		setUpPlayerUIPane("Player4");


		//PlayerUIPane playerPane = new PlayerUIPane();
		//playerPane = transform.Find("Player5").gameObject.AddComponent<PlayerUIPane>();
		//playerPane.initVals("Player5");

		//playerPane = new PlayerUIPane();
		//playerPane = transform.Find("Player2").gameObject.AddComponent<PlayerUIPane>();
		//playerPane.initVals("Player2");
	}

	void setUpPlayerUIPane(string pl){
		PlayerUIPane playerPane = transform.Find(pl).gameObject.AddComponent<PlayerUIPane>();
		playerPane.initVals(pl);
	}
}
