using UnityEngine;
using System.Collections;

public class Output_ItemObject : MonoBehaviour {
	//	GameObject itemObject; //object we're messing with
	Vector3 rotation;
	Vector3 position;
	Shader focusedShader;
	Shader nonFocusedShader;
	
	static ItemClass itemClass = new ItemClass ();
	
	void Start(){
		focusedShader = Shader.Find("Toon/Lighted Outline");
		nonFocusedShader = Shader.Find("Bumped Diffuse");
		setToNonFocused();
	}
	
	void Update () {
		this.gameObject.transform.position = position;
		this.gameObject.transform.eulerAngles = rotation;
	}
	
	
	//focus	
	
	
	public void setToFocused(){
		//		this.gameObject.renderer.material.shader = focusedShader;
		//		this.gameObject.renderer.material.SetColor("_OutlineColor", Color.yellow);
	}
	
	public void setToNonFocused(){
		//		this.gameObject.renderer.material.shader = nonFocusedShader;
	}
	
	
	//position
	
	public void changePosition(Vector3 newPos){
		position = newPos;
	}
	
	public Vector3 getPosition(){
		return this.gameObject.transform.position;
	}
	
	
	//rotation
	
	public Vector3 getRotation(){
		//		Debug.Log (this.gameObject.transform.rotation.eulerAngles);
		return this.gameObject.transform.eulerAngles;
		
		
	}
	
	public void rotate(float deg){
		rotation.x = 0f;
		rotation.z = 0f;
		rotation.y += deg;
		
		itemClass.modifyItemList(getName(), getPosition(), rotation);
		//		Debug.Log (getRotation());
	}
	
	public void changeOrientation(Vector3 newRot){
		rotation = newRot;
		//		this.gameObject.transform.eulerAngles = newRot;
	}
	
	
	//name
	
	public string getName(){
		return this.gameObject.name;
	}
	
	public void setName(string s){
		this.gameObject.name = s;
	}
	
	public GameObject getGameObject(){
		return this.gameObject;
	}
}
