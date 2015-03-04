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
	//     returns false if it fails
	public bool findShortestPathTo(Node<T> toNode, Node<T> fromNode) {
		if (!this.allNodes.Contains (toNode)) Debug.LogWarning ("Goal node with value " + toNode.value + " does not exist in graph");
		else if (toNode.neighbors.Count == 0) Debug.LogWarning ("Goal node with value " + toNode.value + " does not have any paths conencted to it");
		else if (!this.allNodes.Contains(fromNode)) Debug.LogWarning ("Source node with value " + fromNode.value + " does not exist in graph");
		else if (fromNode.neighbors.Count == 0) Debug.LogWarning ("Source node with value " + fromNode.value + " does not have any paths conencted to it");
		else return this.Dijkstra(toNode, fromNode);
		return false;
	}

	// Dijkstra algorthm to find shortest path to a node from current node
	//     Path is set within the node itself
	//     returns false if it fails
	protected bool Dijkstra(Node<T> toNode, Node<T> fromNode) {
		toNode.distanceToSource = 0.0f;
		toNode.nodeToSource = null;

		List<Node<T>> nodesToVisit = new List<Node<T>> ();

		// Start create our queue
		foreach(Node<T> vertex in this.allNodes) {
			if (vertex != toNode) {
				vertex.distanceToSource = Mathf.Infinity;
				vertex.nodeToSource = null;
			}
			nodesToVisit.Add(vertex);
		}

		float tempDist = 0.0f;
		Node<T> onNode = nodesToVisit[0];
		List<Edge<T>> neighbors;

		// Primary loop for calculating path
		while (nodesToVisit.Count > 0) {
			// Find current node with shortest distance from goal node
			foreach (Node<T> value in nodesToVisit) {
				if (value.distanceToSource < onNode.distanceToSource) onNode = value;
			}

			// We have found our shortest path to the source from our node
			if (onNode.Equals(fromNode)) break;

			nodesToVisit.Remove(onNode);
			neighbors = onNode.neighbors;

			foreach (Edge<T> edge in neighbors) {
				tempDist = onNode.distanceToSource + edge.length;
				if (tempDist < edge.toNode.distanceToSource) {
					edge.toNode.distanceToSource = tempDist;
					edge.toNode.nodeToSource = onNode;
				}
			}
		}

		if (onNode.Equals (fromNode)) return true;

		Debug.LogWarning ("Could not find a path from source node with value " + fromNode.value + " to goal node with value " + toNode.value);
		return false;
	}

	//------------------//
	
	
}