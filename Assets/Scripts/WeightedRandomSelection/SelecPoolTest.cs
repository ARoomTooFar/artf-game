using UnityEngine;
using System.Collections;

public class SelecPoolTest : MonoBehaviour {

	float reps = 100000;

	// Use this for initialization
	void Start () {
		SelectionPoolTest();
		SelectionPoolUnweightedTest();
	}

	void Update(){

	}

	private void SelectionPoolTest(){
		SelectionPool<string> pool = new SelectionPool<string>();
		pool.addItem(new SelectionItem<string>("test1"), 5)
		.addItem(new SelectionItem<string>("test2"), 4)
		.addItem(new SelectionItem<string>("test3"), 1);

		int test1 = 0;
		int test2 = 0;
		int test3 = 0;

		string val;
		for(int i = 0; i < reps; ++i) {
			val = pool.getItem().openItem();
			//print(val);
			switch(val){
			case "test1":
				test1++;
				break;
			case "test2":
				test2++;
				break;
			case "test3":
				test3++;
				break;
			}
		}

		print(string.Format("{0}, {1}, {2}", test1 / reps * 100.0, test2 / reps * 100.0, test3 / reps * 100.0));
	}

	private void SelectionPoolUnweightedTest(){
		SelectionPoolUnweighted<string> pool = new SelectionPoolUnweighted<string>();
		pool.addItem(new SelectionItem<string>("test1"), 5)
		.addItem(new SelectionItem<string>("test2"), 4)
		.addItem(new SelectionItem<string>("test3"), 1);
		
		int test1 = 0;
		int test2 = 0;
		int test3 = 0;
		
		string val;
		for(int i = 0; i < reps; ++i) {
			val = pool.getItem().openItem();
			//print(val);
			switch(val){
			case "test1":
				test1++;
				break;
			case "test2":
				test2++;
				break;
			case "test3":
				test3++;
				break;
			}
		}
		
		print(string.Format("{0}, {1}, {2}", test1 / reps * 100.0, test2 / reps * 100.0, test3 / reps * 100.0));
	}

}
