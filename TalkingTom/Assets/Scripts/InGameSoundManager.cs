using UnityEngine;
using System.Collections;

public class InGameSoundManager : MonoBehaviour {
	public AudioClip snoring , fart, yippee, brush, tickling , loo , jump , flush , gym , slap , punch , dance , headhit , wakeUp , towel;
	public static InGameSoundManager _instance;
	// Use this for initialization
	void Start () {
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
