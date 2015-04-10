/*
 * Taken from http://unitycoder.com/blog/2013/01/26/save-mesh-created-by-script-in-editor-playmode/
 */

using UnityEngine;
using UnityEditor;
using System.Collections;

// assetSaver v1.0 - attach this script to any object, assign the target transfrom (from where the mesh is saved),
// give some filename. Play. Then press F to save.

public class saveMesh : MonoBehaviour
{
	
	public string name1;
	public Transform obj1;
	
	void Update ()
	{
		if (Input.GetKeyDown("f"))
		{
			SaveAsset();
		}
	}
	
	void SaveAsset()
	{
		Mesh m1 = obj1.GetComponent<MeshFilter>().mesh;
		AssetDatabase.CreateAsset(m1, "Assets/" + name1 + ".asset"); // saves to "assets/"
		//AssetDatabase.SaveAssets(); // not needed?
	}
	
}