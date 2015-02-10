using UnityEngine;
using System.Collections;

public class AggroNode 
{
	private GameObject g;
	private int p;
	public AggroNode l;
	public AggroNode r;

	public AggroNode(GameObject g1, int p1)
	{
		g = g1;
		p = p1;
	}
	public int getPrio()
	{
		return p;
	}
	public GameObject getObj()
	{
		return g;
	}
	public void changePrio(int n)
	{
		p = n;
	}
}

//Aggro table class for enemies
public class AggroTable 
{
	private AggroNode head;
	private AggroNode tail;
	public bool testing = false;

	public void add(GameObject g1, int p1)
	{
		AggroNode n = new AggroNode (g1, p1);

		AggroNode runner = head;
		while (runner != null)
		{
			AggroNode temp = runner;
			runner = runner.r;
			if (g1 == temp.getObj ())
			{
				deleteNode (temp);
			}
		}

		if (head == null) {
			head = n;
			tail = n;
		}else {
			addR (n, head);
		}
	}


	public void addR(AggroNode nAdd, AggroNode nOld){
		if (nAdd.getPrio () > nOld.getPrio ()) 
		{
			if (nOld.l != null) 
			{
				nOld.l.r = nAdd;
			}
			else
			{
				head = nAdd;
			}
			nAdd.l = nOld.l;
			nAdd.r = nOld;
			nOld.l = nAdd;
		} 
		else if (nOld.r == null) 
		{
			nOld.r = nAdd;
			nAdd.l = nOld;
			tail = nAdd;
		} 
		else 
		{
			addR (nAdd, nOld.r);
		}
	}

	public void deleteNode(AggroNode del)
	{
		if (testing)
			Debug.Log ("Deleting " + del.getPrio());
		//Case node given is null
		if (del == null) 
		{
			return;
		}
		//Case node was head
		else if (del.l == null) 
		{
			head = del.r;
			//If new head is null, so is tail
			//If new head does not point to anything, tail is head
			if(head == null || head.r == null)
			{
				tail = head;
			}
		}
		//Case node is in middle, not head nor tail
		else if (del.r != null)
		{
			if(testing)
				Debug.Log (del.l.getPrio() + " is to the left of the deleted node");
			del.l.r = del.r;
			del.r.l = del.l;
		}
		//Case node is tail
		else
		{
			del.l.r = null;
			tail = del.l;
		}
		del = null;
	}


	public void deletePlayer(GameObject del)
	{
		AggroNode runner = head;
		while (runner != null)
		{
			AggroNode temp = runner;
			runner = runner.r;
			if (del == temp.getObj ())
			{
				deleteNode (temp);
			}
		}
	}

	public GameObject getTarget()
	{
		return head.getObj ();
	}

	public int getVal()
	{
		return head.getPrio ();
	}

	public void printOrder()
	{
		AggroNode runner = head;
		while (runner != null) {
			Debug.Log (runner.getPrio ());
			runner = runner.r;
		}
	}
}
