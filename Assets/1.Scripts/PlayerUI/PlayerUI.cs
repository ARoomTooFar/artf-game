using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//initializes the 4 player panes
public class PlayerUI : MonoBehaviour {

	void Start () {

		//hardcoded just for the test scene
		//PlayerUIPane playerPane = new PlayerUIPane();
		//playerPane = transform.Find("Player1").gameObject.AddComponent<PlayerUIPane>();
<<<<<<< HEAD
		//playerPane.initVals("Player1");
=======
		//playerPane.initVals("PlayerZ");
>>>>>>> 039de27056e39a1d09758e5fbff0801fa2174a01

		//for scenes with all 4 players
		setUpPlayerUIPane("Player1");
		setUpPlayerUIPane("Player2");
		setUpPlayerUIPane("Player3");
		setUpPlayerUIPane("Player4");
<<<<<<< HEAD

		//PlayerUIPane playerPane = new PlayerUIPane();
		//playerPane = transform.Find("Player5").gameObject.AddComponent<PlayerUIPane>();
		//playerPane.initVals("Player5");

		//playerPane = new PlayerUIPane();
		//playerPane = transform.Find("Player2").gameObject.AddComponent<PlayerUIPane>();
		//playerPane.initVals("Player2");
=======
>>>>>>> 039de27056e39a1d09758e5fbff0801fa2174a01

	}

	void setUpPlayerUIPane(string pl){
		Debug.Log (pl);
		PlayerUIPane playerPane = new PlayerUIPane();
		playerPane = transform.Find(pl).gameObject.AddComponent<PlayerUIPane>();
		playerPane.initVals(pl);

	}
}
