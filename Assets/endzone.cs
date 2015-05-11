using UnityEngine;
using System.Collections;

//If a player enters the end zone it will add 1 to the number of players in the endzone.
//If a player leaves the end zone it will subtract 1 from the number of players in the endzone.
//It will move on to the reward scene if all the players that are alive have entered the end zone.
public class endzone : MonoBehaviour {
	//
	public int numbPlayers = 4;
	public int numbPlayersInZone = 0;
	public int numbPlayersAlive = 4;
	public GameObject[] plrs;

	//private Player player;

	//void Awake
	//{
	//	player =  GetComponent<Player>();
	//}


	//-----------------------------------------
	//OnTriggerEnter()
	//-----------------------------------------
	//When a player enters the zone the level will end.
	//-----------------------------------------
	void OnTriggerEnter(Collider other) 
	{
		//the tag of the thing colliding.
		string t = other.GetComponent<Collider>().tag;

		//if the list of players for the scene is not initialized.
		if (plrs == null) 
		{
			plrs = GameObject.FindGameObjectsWithTag ("Player"); //finds all objects in the scene with the "Player" tag and typecasts them as player.
			numbPlayers = plrs.Length;
			print ("Initializing player list, numbPlayers is: " + numbPlayers);
		}

		if (t == "Player") 
		{
			numbPlayersInZone ++;
			print ("+Number of Players in end zone: " + numbPlayersInZone + ".");

		}
		if (numbPlayers == numbPlayersInZone) 
		{
			print("All Players are in the End Zone.");
		}
	}
	//---------------------------------------
	//OnTriggerExit(Collider other)
	//---------------------------------------
	//If player, subtract from the number of players in the zone when a player leaves.
	//---------------------------------------
	void OnTriggerExit(Collider other)
	{
		string t = other.GetComponent<Collider>().tag;
		if (t == "Player") 
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
		numbPlayersAlive = 0;

		for(int i = 0; i <= plrs.Length; i++)
		{

			//plr = gameObject.GetComponent<Player>();

			//if(plr != null && !plr.isDead)
			//{
			//		numbPlayersAlive++;
			//}
			
		}
	}
}
