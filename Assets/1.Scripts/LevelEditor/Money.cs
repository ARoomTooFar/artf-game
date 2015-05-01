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
		money = 3000;
		priceTable = new Dictionary<string, int> ();
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

	public static bool buy (string s, int price)
	{
		if(price <= money){
			money -= price;
			updateMoneyDisplay();
			return true;
		}else{
			Debug.Log("Insufficient funds. Construct more pylons.");
			return false;
		}
	}
}
