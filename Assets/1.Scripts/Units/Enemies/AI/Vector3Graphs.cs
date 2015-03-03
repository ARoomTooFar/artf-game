// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-----------------------------//
// Structs for our Graph Class //
//-----------------------------//

public struct V3Node {
	private struct V3Edge {
		private float _length;
		public float length {
			get {return this._length;}
			private set {this._length = value;}
		}
		
		private V3Node _toNode;
		public V3Node toNode {
			get {return this._toNode;}
			private set {this._toNode = value;}
		}
		
		public V3Edge (float length, V3Node toNode) {
			this.length = length;
			this.toNode = toNode;
		}
	}
	
	private List<V3Edge> neighbors;
	
	private Vector3 _position;
	public Vector3 position {
		get {return this._position;}
		private set {this._position = value;}
	}
	
	
	
	public V3Node (Vector3 position) {
		this.position = position;
		this.neighbors = new List<V3Edge>();
	}
	
	public void findNeighbors (List<V3Node> possibleNeighbors, Rigidbody unitToTest) {
		RaycastHit[] hits;
		Vector3 direction;
		foreach (V3Node node in possibleNeighbors) {
			direction = this.position - node.position;
			direction.y = 0.0f;
			direction.Normalize ();
			
			hits = unitToTest.SweepTestAll(direction, Vector3.Distance(this.position, node.position));
			
			for (int i = 0; i < hits.Length; ++i) {
				if (hits[i].collider.tag == "Wall") break;
				if (i == hits.Length) {
					this.addNeighbor(node);
					node.addNeighbor(this);
				}
			}
		}
	}
	
	public void addNeighbor (V3Node newNeighbor) {
		V3Edge newEdge = new V3Edge(Vector3.Distance(this.position, newNeighbor.position), newNeighbor);
		this.neighbors.Add (newEdge);
	}
}

//--------------------------------//



public class Vector3Graphs {
	
	//-----------//
	// Variables //
	//-----------//

	private List<V3Node> allNodes;

	//-----------//


	//------------------//
	// Public Functions //
	//------------------//

	public Vector3Graphs() {
		this.allNodes = new List<V3Node>();
	}

	// Adds the new position to our graph
	public V3Node addNode(Vector3 position, Rigidbody unitToTest) {
		V3Node newNode = new V3Node(position);
		newNode.findNeighbors(allNodes, unitToTest);
		allNodes.Add (newNode);
		return newNode;
	}

	// Adds new position node to our graph
	public void addNode (V3Node newNode, Rigidbody unitToTest) {
		newNode.findNeighbors (allNodes, unitToTest);
		allNodes.Add (newNode);
	}
	
	// Finds the shortest path to the given node
	public void findShortestPathTo(V3Node toNode) {
		if (!this.allNodes.Contains(toNode)) Debug.LogWarning ("Specified V3Node does not exist in list");
		else {

		}
	}


	//------------------//


}