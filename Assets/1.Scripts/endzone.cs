using UnityEngine;
using System.Collections;

//If a player enters the end zone it will add 1 to the number of players in the endzone.
//If a player leaves the end zone it will subtract 1 from the number of players in the endzone.
//It will move on to the reward scene if all the players that are alive have entered the end zone.
//FOR THIS SCRIPT ALL PLAYERS MUST BE TAGED AS "Player"
public class endzone : MonoBehaviour {
	
	public int numbPlayersInZone = 0;
	public int numbPlayersAlive;
	public Player[] players;
	public string endScene = "RewardScreen";
	private Character character;

	private GSManager gsManager;

	void Start(){
		gsManager = null;
		try {
			gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		} catch {}
	}

	//-----------------------------------------
	//OnTriggerEnter()
	//-----------------------------------------
	//When a player enters the zone the level will end.
	//-----------------------------------------
	void OnTriggerEnter(Collider other) 
	{
		//if the list of players for the scene is not initialized.
		if (players == null || players.Length == 0) 
		{
			print ("Checked if null...");
			players = gsManager.players;
		}

		string t = other.tag;


		if (t == "Player" || t == "Player1" || t == "Player2" || t == "Player3" || t == "Player4") 
		{
			checkAlive();
			numbPlayersInZone ++;
			print ("+Number of Players in end zone: " + numbPlayersInZone + ".");
			if (numbPlayersAlive == numbPlayersInZone) 
			{
				gsManager.LoadScene("PlayAgainScene");
			}
		}
		checkAlive();
	}
	//---------------------------------------
	//OnTriggerExit(Collider other)
	//---------------------------------------
	//If player, subtract from the number of players in the zone when a player leaves.
	//---------------------------------------
	void OnTriggerExit(Collider other)
	{
		string t = other.tag;
		if (t == "Player" || t == "Player1" || t == "Player2" || t == "Player3" || t == "Player4") 
		{
			numbPlayersInZone --;
			print ("-Number of Players in end zone: " + numbPlayersInZone + ".");
		}
	}

	//---------------------------------------
	//checkAlive()
	//---------------------------------------
	//Checks to see how many players in the scene are alive.
	//---------------------------------------
	void checkAlive ()
	{
		print ("Bring out yer dead!!!");
		numbPlayersAlive = 0;
		for(int i = 0; i < players.Length; i++)
		{
			if (players[i] != null) {
				//this gets the character component of the player
				character = players[i].GetComponent<Character>();

				//checks if the character is dead or not
				if(character != null && !character.isDead)
				{
						//if not add to number of players alive.
						numbPlayersAlive++;
				}
				//print("There are " + numbPlayersAlive + " Players alive.");
			}
		}
		print("eThere are " + numbPlayersAlive + " Players alive.");
	}
}
