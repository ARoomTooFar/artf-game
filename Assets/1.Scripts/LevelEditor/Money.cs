using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Handles money.
//Initialization for some of these things in in the Start
//function in FolderBarController.cs
public static class Money {
	public static int money;
	public static Text moneyDisplay;

	static Money() {
		money = 3000;
	}

	public static void updateMoneyDisplay() {
		moneyDisplay.text = ("$" + money);
	}

	public static bool buy(int price) {
		if(price <= money) {
			money -= price;
			updateMoneyDisplay();
			return true;
		} else {
			Debug.Log("Insufficient funds. Construct more pylons.");
			return false;
		}
	}
}
