//Andrew Miller
//Created 1/17/15

using UnityEngine;
using System.Collections;

public class levelgui : MonoBehaviour {

	private string levelName;


	// Initialize scene
	void Start () 
	{
		levelName = gamestate.Instance.getLevel ();
		print ("Loaded: " + gamestate.Instance.getLevel ());
	}
	
	//--------------------------------
	//OnGUI()
	//--------------------------------
	//Provides a GUI in scenes
	//--------------------------------
	void OnGUI()
	{
	
		//levelName = gamestate.Instance.getLevel ();

		switch (levelName)
		{
			case "PlayerSelect":
				//creates button to move between playerselect and levelselect
				if (GUI.Button (new Rect (30, 30, 150, 30), "Level Select")) 
				{
					moveToScene ("LevelSelect");
				}
				break;
			case "LevelSelect":
				//creates button to move between LevelSelect and InventorySelect
				if (GUI.Button (new Rect (30, 30, 150, 30), "Inventory Select"))
				{
					moveToScene ("InventorySelect");
				}
				break;
			case "InventorySelect":
				//creates button to move between InventorySelect and LevelSelect
				if (GUI.Button (new Rect (30, 30, 150, 30), "Game Scene"))
				{
					moveToScene ("GameScene");
				}
				break;
			case "GameScene":
				
				break;
		}
	}

	//--------------------------------
	//moveToScene()
	//--------------------------------
	//Moves to specified scene which is passed as a string.
	//--------------------------------
	void moveToScene(string aScene)
	{
		levelName = aScene;
		print("moving to "+ aScene);
		gamestate.Instance.setLevel(aScene);
		DontDestroyOnLoad (gamestate.Instance);
		Application.LoadLevel(aScene);
	}

}
