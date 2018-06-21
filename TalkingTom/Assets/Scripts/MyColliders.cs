using UnityEngine;
using System.Collections;

public class MyColliders : MonoBehaviour {
	public BoxCollider tummy,leftCheek,rightCheek,head,body;
	// Use this for initialization
	void Start () {
		body.enabled = false;
		if(Application.loadedLevelName == "BedRoom")
		{
			tummy.enabled = false;
			rightCheek.enabled = false;
			leftCheek.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
