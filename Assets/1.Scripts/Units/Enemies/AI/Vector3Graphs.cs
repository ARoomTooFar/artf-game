// An inherited Graph thatr is used for Vector 3 values
//     Made specifically for pathing back to initial point

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vector3Graphs : Graphs<Vector3> {
	
	//-----------//
	// Variables //
	//-----------//

	private Rigidbody unit;

	//-----------//


	//------------------//
	// Public Functions //
	//------------------//

	public Vector3Graphs(Rigidbody unit) {
		this.unit = unit;
	}


	// Overwritten functions
	// Adds the new node with T value to our graph
	public override Node<Vector3> addNode(Vector3 value) {
		Node<Vector3> newNode = new Node<Vector3>(value);
		this.findNeighbors (newNode);
		allNodes.Add (newNode);
		return newNode;
	}
	
	// Adds new node to our graph
	public override void addNode (Node<Vector3> newNode) {
		newNode.clearNeighbors ();
		this.findNeighbors (newNode);
		this.allNodes.Add (newNode);
	}

	// Vector3Graph functions
	
	protected void findNeighbors (Node<Vector3> newNode) {
		RaycastHit[] hits;
		Vector3 direction;
		foreach (Node<Vector3> node in this.allNodes) {
			direction = newNode.value - node.value;
			direction.y = 0.0f;
			direction.Normalize ();
			
			hits = this.unit.SweepTestAll(direction, Vector3.Distance(newNode.value, node.value));
			
			for (int i = 0; i < hits.Length; ++i) {
				if (hits[i].collider.tag == "Wall") break;
				if (i == hits.Length - 1) {
					newNode.addNeighbor(node);
					node.addNeighbor(newNode);
					Debug.Log("adding neighbor");
				}
			}
		}
	}

	//------------------//


}