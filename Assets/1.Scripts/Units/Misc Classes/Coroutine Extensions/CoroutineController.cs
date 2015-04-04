// Class that adds more functionanilty to Unity's coroutines
// From http://forum.unity3d.com/threads/extended-coroutines.202064/

using UnityEngine;
using System.Collections;

public enum CoroutineState
{
	Ready,
	Running,
	Paused,
	Finished
}

public class CoroutineController {
	public delegate void OnFinish(CoroutineController coroutineController);
	
	public event OnFinish onFinish;
	
	private IEnumerator _routine;
	private Coroutine _coroutine;  
	private CoroutineState _state;
	
	public CoroutineController(IEnumerator routine) {
		_routine = routine;
		_state = CoroutineState.Ready;
	}
	
	public void StartCoroutine(MonoBehaviour monoBehaviour) {
		_coroutine = monoBehaviour.StartCoroutine(Start());
	}
	
	private IEnumerator Start() {
		if (_state != CoroutineState.Ready)
		{
			throw new System.InvalidOperationException("Unable to start coroutine in state: " + _state);
		}
		
		_state = CoroutineState.Running;
		while (_routine.MoveNext())
		{
			yield return _routine.Current;
			while (_state == CoroutineState.Paused)
			{
				yield return null;
			}
			if (_state == CoroutineState.Finished)
			{
				yield break;
			}
		}
		
		_state = CoroutineState.Finished;
		
		if(onFinish != null)
			onFinish(this);
	}
	
	public void Stop() {
		if (_state != CoroutineState.Finished) {
			if (_state != CoroutineState.Running  && _state != CoroutineState.Paused) {
				throw new System.InvalidOperationException("Unable to stop coroutine in state: " + _state);
			}
			
			_state = CoroutineState.Finished;
		}
	}
	
	public void Pause() {
		if (_state != CoroutineState.Running) {
			throw new System.InvalidOperationException("Unable to pause coroutine in state: " + _state);
		}
		
		_state = CoroutineState.Paused;
	}
	
	public void Resume() {
		if (_state != CoroutineState.Paused) {
			throw new System.InvalidOperationException("Unable to resume coroutine in state: " + state);
		}
		
		_state = CoroutineState.Running;
	}

	public CoroutineState state {
		get { return _state;}
	}
	
	public Coroutine coroutine {
		get { return _coroutine; }
	}
	
	public IEnumerator routine {
		get { return _routine; }
	}

	public bool isFinished() {
		if (_state == CoroutineState.Finished) {
			return true;
		}
		return false;
	}
}

