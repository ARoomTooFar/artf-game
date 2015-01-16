using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

//This class utilizes some of Unity's handlers to make sure the UI attached
//to an object has visibility at the right times
public class ObjectClickingHandler : MonoBehaviour, IPointerClickHandler, IEndDragHandler
{
	public GameObject UIItems; //holds the object UI
	
	public void OnPointerClick (PointerEventData data)
	{
		UIItems.SetActive (!UIItems.activeSelf); //toggle the object UI
	}

	public void OnEndDrag (PointerEventData data)
	{
		//if the object is being dragged, when the mouse lets up, it will count as a click.
		//to stop this from changing the state of the UI, we toggle the UI again 
		//to reverse the effect
		UIItems.SetActive (!UIItems.activeSelf);
	}
}
