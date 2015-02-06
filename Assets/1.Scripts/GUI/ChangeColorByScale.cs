using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
// Script that Lerp the color of a image depending of the scale of the transform
public class ChangeColorByScale : MonoBehaviour {
 
 public enum SelectedAxis{
  xAxis,
  yAxis,
  zAxis
 }
 public SelectedAxis selectedAxis = SelectedAxis.xAxis;
 
 // Target
 public Image image;
 
 // Parameters
 public float minValue = 0.0f;
 public float maxValue = 1.0f;
 public Color minColor = Color.red;
 public Color maxColor = Color.green;
 
 // The default image is the one in the gameObject
 void Start(){
  if (image == null){
   image = GetComponent<Image>();
  }
 }
  
 void Update () {
  switch (selectedAxis){
  case SelectedAxis.xAxis:
   // Lerp color depending on the scale factor
   image.color = Color.Lerp(minColor,
                            maxColor,
                            Mathf.Lerp(minValue,
                     maxValue,
                     transform.localScale.x));
   break;
  case SelectedAxis.yAxis:
   // Lerp color depending on the scale factor
   image.color = Color.Lerp(minColor,
                            maxColor,
                            Mathf.Lerp(minValue,
                     maxValue, transform.localScale.y));
   break;
  case SelectedAxis.zAxis:
   // Lerp color depending on the scale factor
   image.color = Color.Lerp(minColor,
                            maxColor,
                            Mathf.Lerp(minValue,
                     maxValue,
                     transform.localScale.z));
   break;
  }
 }
}