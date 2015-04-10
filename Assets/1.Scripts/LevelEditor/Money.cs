using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public static class Money
{
	public static int money;
	public static Text moneyDisplay;
	static Dictionary<string, int> priceTable;

	static Money ()
	{
		money = 300;

//		moneyDisplay = GameObject.Find ("Shop").GetComponentInChildren<Text> () as Text;
//		updateMoneyDisplay();

		priceTable = new Dictionary<string, int> ();

		//misc
		priceTable.Add ("Alocasia1", 20);
		priceTable.Add ("Alocasia2", 90);
		priceTable.Add ("bananatree1", 30);
		priceTable.Add ("bananatree2", 10);

		//monster
		priceTable.Add ("BullyTrunk", 20);
		priceTable.Add ("Artilitree", 90);
		priceTable.Add ("CackleBranch", 30);
		priceTable.Add ("FoliantFodder", 10);
	}

	public static void updateMoneyDisplay(){
		moneyDisplay.text = ("$" + money.ToString ());
	}

	public static int getPrice(string s){
		if (priceTable.ContainsKey (s)) {
			return Money.priceTable[s];
		}else{
			return 69;
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
