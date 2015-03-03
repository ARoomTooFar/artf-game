// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vector3Graphs {

	//-----------------------------//
	// Structs for our Graph Class //
	//-----------------------------//

	private struct V3Node {
		private struct V3Edge {
			private float _length;
			public float length {
				public get {return this._length;}
				private set {this._length = value;}
			}
			
			private V3Node _toNode;
			public V3Node toNode {
				public get {return this._toNode;}
				private set {this._toNode = value;}
			}
			
			public V3Edge (float length, V3Node toNode) {
				this.length = length;
				this.toNode = toNode;
			}
		}

		private Vector3 _position;
		public Vector3 position {
			public get {return this._position;}
			private set {this._position = value;}
		}

		private List<V3Edge> neighbors;

		public V3Node (Vector3 position) {
			this.position = position;
			this.neighbors = new List<V3Edges>();
		}

		public void findNeighbors (List<V3Node> possibleNeighbors, Rigidbody unitToTest) {
		}

		//------------------------------//
	}




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

	//------------------//


}