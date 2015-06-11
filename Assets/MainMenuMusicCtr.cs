using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MainMenuMusicCtr : MonoBehaviour {

	public AudioMixerSnapshot login;
	public AudioMixerSnapshot levelselect;
	
	void Start () {
	
	}

	public void playLogIn(float transitionIn) {
		login.TransitionTo (transitionIn);
	}

	public void playLvSelect(float transitionIn) {
		levelselect.TransitionTo (transitionIn);
	}
}
