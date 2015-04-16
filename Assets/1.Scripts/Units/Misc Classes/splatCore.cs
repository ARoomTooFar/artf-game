using UnityEngine;
using System.Collections;

public class splatCore : MonoBehaviour {
	public GameObject drip;
	// Use this for initialization
	void Start () {
	
	}
	void Awake(){
		RaycastHit hit;
		int x = -1;
		//int drops = Random.Range(40,80);
		int drops = Random.Range(100,200);
		while(x<=drops){
			x++;
			Vector3 fwd = transform.TransformDirection(Random.onUnitSphere*5);
			if(Physics.Raycast(transform.position, fwd, out hit, 10)){
				GameObject splatter = ((GameObject)Instantiate(drip,hit.point + (hit.normal *0.1f),Quaternion.FromToRotation(Vector3.up, hit.normal)));
				float scaler = Random.value;
                splatter.transform.localScale = new Vector3(splatter.transform.localScale.x*scaler,splatter.transform.localScale.y,splatter.transform.localScale.z*scaler);
                
                int rater = Random.Range (0, 359);
                splatter.transform.RotateAround (hit.point, hit.normal, rater);
                
                Destroy (splatter, 5);
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
