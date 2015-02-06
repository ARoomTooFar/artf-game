using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class controls the circle HP ring
public class LifeBar: MonoBehaviour
{
	public float max;
	public float curr;
	void Update () { 
		renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, curr/max)); 
	}
}
