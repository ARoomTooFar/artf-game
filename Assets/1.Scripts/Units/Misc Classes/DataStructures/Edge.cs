//------------//
// Edge Class //
//------------//

public class Edge<T> {
	public float _length;
	public float length {
		get {return this._length;}
		protected set {this._length = value;}
	}
	
	protected Node<T> _toNode;
	public Node<T> toNode {
		get {return this._toNode;}
		protected set {this._toNode = value;}
	}
	
	public Edge (float length, Node<T> toNode) {
		this.length = length;
		this.toNode = toNode;
	}
}

//----------//