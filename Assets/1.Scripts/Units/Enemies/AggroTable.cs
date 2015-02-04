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
}

//Aggro table class for enemies
public class AggroTable 
{
	private AggroNode head;
	private AggroNode tail;

	public void add(GameObject g1, int p1)
	{
		AggroNode n = new AggroNode (g1, p1);
		if (head == null) {
			head = n;
			tail = n;
		} else {
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

	public void updateVal(GameObject player, int newVal)
	{
		//Update AggroNode values, and rearrange node order
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
