using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSoap : MonoBehaviour {
	public static CharacterSoap _instance;
//	public SpriteRenderer []mySoap;
	public List <SpriteRenderer> mySoap = new List<SpriteRenderer>();
	// Use this for initialization
	void Awake () {
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
