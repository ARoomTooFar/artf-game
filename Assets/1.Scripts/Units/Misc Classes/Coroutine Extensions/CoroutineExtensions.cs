// Class that adds more functionanilty to Unity's coroutines
// From http://forum.unity3d.com/threads/extended-coroutines.202064/

using UnityEngine;
using System.Collections;

public static class CoroutineExtensions
{
	public static Coroutine StartCoroutineEx(this MonoBehaviour monoBehaviour, IEnumerator routine, out CoroutineController coroutineController)
	{
		if (routine == null)
		{
			throw new System.ArgumentNullException("routine");
		}
		
		coroutineController = new CoroutineController(routine);
		coroutineController.StartCoroutine(monoBehaviour);
		return coroutineController.coroutine;
		// return monoBehaviour.StartCoroutine(coroutineController.Start());
	}
}

/*
public static class CoroutineExtensions {
	public static CoroutineController StartCoroutineEx(this MonoBehaviour monoBehaviour, IEnumerator routine) {
		if (routine == null) {
			throw new System.ArgumentNullException("routine");
		}
		
		CoroutineController coroutineController = new CoroutineController(routine);
		coroutineController.StartCoroutine(monoBehaviour);
		return coroutineController;
	}
}*/