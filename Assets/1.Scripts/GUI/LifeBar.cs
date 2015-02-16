using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class controls the circle HP ring
public class LifeBar: MonoBehaviour
{
	public float max;
	public float current;
	void Update () { 
		renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
	}
}
