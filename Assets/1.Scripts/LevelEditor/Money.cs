using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Handles money.
//Initialization for some of these things in in the Start
//function in FolderBarController.cs
public static class Money
{
	public static int money;
	public static Text moneyDisplay;
	static Dictionary<string, int> priceTable;

	static Money ()
	{
		money = 300;

		priceTable = new Dictionary<string, int> ();

		//misc
//		priceTable.Add ("Alocasia1", 20);
//		priceTable.Add ("Alocasia2", 90);
//		priceTable.Add ("bananatree1", 30);
//		priceTable.Add ("bananatree2", 10);
//
//		//monster
//		priceTable.Add ("BullyTrunk", 20);
//		priceTable.Add ("Artilitree", 90);
//		priceTable.Add ("CackleBranch", 35);
//		priceTable.Add ("FoliantFodder", 10);
//
//		//room
//		priceTable.Add ("doortile", 20);
//		priceTable.Add ("floortile", 5);
//		priceTable.Add ("wallstoneend", 30);
//		priceTable.Add ("wallstonestraight", 10);
//		priceTable.Add ("walltile", 10);
//
//		//traps
//		priceTable.Add ("Dart Trap", 20);
//		priceTable.Add ("FlamePit", 30);
//		priceTable.Add ("SlowField", 30);
//		priceTable.Add ("SpikeTrap", 10);
//		priceTable.Add ("StunTrap", 10);
	}

	public static void updateMoneyDisplay(){
		moneyDisplay.text = ("$" + money.ToString ());
	}

	public static int getPrice(string s){
		if (priceTable.ContainsKey (s)) {
			return Money.priceTable[s];
		}else{
			return -1;
		}
	}

	public static void buy (string s)
	{
		if (priceTable.ContainsKey (s)) {
			money -= priceTable [s];
			updateMoneyDisplay();
		}else{
			Debug.Log("No price data for that item");
		}
	}
}
