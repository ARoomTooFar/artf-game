using UnityEngine;
using System.Collections;

//Aggro table class for enemies
public class AggroTable {

	// Aggro Node class
	//     Holds the gameobject with aggro and the neighboring nodes
	private class AggroNode {
		public GameObject unit;
		public Player play; // Used for testing for death (May need to derive from damageable in the future instead of Player)
		public int aggro;
		public AggroNode prev;
		public AggroNode next;
		
		public AggroNode(GameObject unit, int aggro) {
			this.unit = unit;
			this.play = unit.GetComponent<Player>();
			this.aggro = aggro;
			this.prev = null;
			this.next = null;
		}

		public void RemoveSelf() {
			this.unit = null;
			this.prev = null;
			this.next = null;
		}
	}

	public int aggroedSize{get; private set;}
	private AggroNode head;
	private AggroNode tail;

	// Constructor
	public AggroTable() {
		aggroedSize = 0;
		head = null;
		tail = null;
	}


	//-------------------//
	// Primary Functions //
	//-------------------//

	// Returns head, which should be the highest aggro
	public GameObject GetTopAggro() {
		if (this.head != null) {
			if (this.head.unit == null || (this.head.play != null && this.head.play.isDead)) {
				this.RemoveUnit(this.head.unit);
				return null;
			}
			return this.head.aggro >= 0 ? this.head.unit : null;
		}
		else return null;
	}

	public int GetAggro() {
		if (this.head != null) return this.head.aggro;
		else return -1;
	}

	// Adds to the aggro if unit is in list, else makes a new node and adds it to the list
	public void AddAggro(GameObject source, int aggro) {
		AggroNode newBlood = this.GetNodeInList(source);
		if (newBlood != null) newBlood.aggro += aggro;
		else {
			newBlood = new AggroNode(source, aggro);
			if (tail != null) {
				tail.next = newBlood;
				newBlood.prev = tail;
			} else {
				head = newBlood;
			}
			tail = newBlood;
			aggroedSize++;
		}
		this.SetRank (newBlood);
	}

	// Like AddAggro but sets aggro to given amount (Stuff like taunt, stealth, flare, etc);
	public void SetAggro(GameObject source, int aggro) {
		AggroNode newBlood = this.GetNodeInList(source);
		if (newBlood != null) newBlood.aggro = aggro;
		else {
			newBlood = new AggroNode(source, aggro);
			if (tail != null) {
				tail.next = newBlood;
				newBlood.prev = tail;
			} else {
				head = newBlood;
			}
			tail = newBlood;
			aggroedSize++;
		}
		this.SetRank (newBlood);
	}

	public void RemoveUnit(GameObject source) {
		AggroNode fool = this.GetNodeInList (source);
		if (fool == null) return;
		if (fool.prev != null) {
			fool.prev.next = fool.next;
			if (fool.next == null) this.tail = fool.prev;
		}
		if (fool.next != null) {
			fool.next.prev = fool.prev;
			if (fool.prev == null) this.head = fool.next;
		}
		fool.RemoveSelf();
		fool = null;
		aggroedSize--;
		if (aggroedSize == 0) this.head = this.tail = null;
	}

	public void ClearTable() {
		AggroNode temp;
		while (head != null) {
			temp = head;
			head = head.next;
			temp.RemoveSelf();
			temp = null;
		}
		aggroedSize = 0;
		this.head = this.tail = null;
	}

	public void PrintTable() {
		string theAssHoles = "";
		AggroNode tempNode = head;
		while (tempNode != null) {
			theAssHoles += tempNode.unit.name + " (" + tempNode.aggro + ") ";
			tempNode = tempNode.next;
		}
		Debug.Log (theAssHoles);
	}

	//------------------//
	// Helper Functions //
	//------------------//

	// Goes through our list and returns the node with the matching target
	//     returns null if none is found
	private AggroNode GetNodeInList(GameObject source) {
		AggroNode tempNode = head;
		while (tempNode != null) {
			if (tempNode.unit != source) tempNode = tempNode.next;
			else break;
		}
		return tempNode;
	}

	// Goes through the neighbors of the passed in node to see if it is in correct place
	private void SetRank(AggroNode unit) {
		// Checks those higher in the list
		while (unit.prev != null) {
			if (unit.prev.aggro > unit.aggro) break;
			else {
				AggroNode prevNode = unit.prev;
				unit.prev = prevNode.prev;
				if (unit.prev != null) unit.prev.next = unit;
				if (unit.next != null) unit.next.prev = prevNode;
				prevNode.prev = unit;
				prevNode.next = unit.next;
				unit.next = prevNode;
				if (prevNode.next == null) this.tail = prevNode;
			}
			if (unit.prev == null) this.head = unit;
		}

		// Checks those lower in the list
		while (unit.next != null) {
			if (unit.next.aggro < unit.aggro) break;
			else {
				AggroNode nextNode = unit.next;
				unit.next = nextNode.next;
				if (unit.next != null) unit.next.prev = unit;
				if (unit.prev != null) unit.prev.next = nextNode;
				nextNode.next = unit;
				nextNode.prev = unit.prev;
				unit.prev = nextNode;
				if (nextNode.prev == null) this.head = nextNode;
			}
			if (unit.next == null) this.tail = unit;
		}
	}
}