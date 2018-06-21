using UnityEngine;
using System.Collections;

public class WaterMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	void Update () {
		transform.position = new Vector3(transform.position.x , transform.position.y - (Time.deltaTime*0.03f) , transform.position.z); // move the water particels down
		if(transform.position.y < -0.25f)
		{
			transform.position = new Vector3(transform.position.x , -0.25f , transform.position.z);
		}
	}
}
