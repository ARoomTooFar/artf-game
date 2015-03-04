using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graphs<T> {

	//-----------//
	// Variables //
	//-----------//
	
	private List<Node<T>> allNodes;
	
	//-----------//
	
	
	//------------------//
	// Public Functions //
	//------------------//
	
	public Graphs() {
		this.allNodes = new List<Node<T>>();
	}
	
	// Adds the new node with T value to our graph
	public virtual Node<T> addNode(T value) {
		Node<T> newNode = new Node<T>(value);
		allNodes.Add (newNode);
		return newNode;
	}
	
	// Adds new node to our graph
	public virtual void addNode (Node<T> newNode) {
		allNodes.Add (newNode);
	}

	/*
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
	}*/
	
	// Finds the shortest path to a node given starting and ending node using Dijstras
	public List<Node<T>> findShortestPathTo(Node<T> toNode) {
		if (!this.allNodes.Contains(toNode)) Debug.LogWarning ("Goal node with value " + toNode.value + " does not exist in graph");
		else return this.Dijkstra(toNode);
		return null;
	}
	
	protected List<Node<T>> Dijkstra(Node<T> toNode) {
		toNode.distanceToSource = 0.0f;
		toNode.nodeToSource = null;

		Queue<Node<T>> queue = new Queue<Node<T>> ();

		// Start create our queue
		foreach(Node<T> vertex in this.allNodes) {
			if (vertex != toNode) {
				vertex.distanceToSource = Mathf.Infinity;
				vertex.nodeToSource = null;
			}
			queue.Enqueue(vertex);
		}

		Node<T> onNode;
		onNode = queue.Peek();

		// Primary loop for calculating path
		while (queue.Count != 0) {
			// Find current node with shortest path
			foreach (Node<T> value in queue) {
				if (value.distanceToSource < onNode.distanceToSource) onNode = value;
			}

			foreach (
		}
	}

	//------------------//
	
	
}