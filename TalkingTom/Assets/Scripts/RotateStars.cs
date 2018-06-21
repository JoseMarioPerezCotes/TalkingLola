using UnityEngine;
using System.Collections;

public class RotateStars : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles = new Vector3(100,transform.eulerAngles.y+(Time.deltaTime*70),0);
	}
}
