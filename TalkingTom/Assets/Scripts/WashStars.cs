using UnityEngine;
using System.Collections;

public class WashStars : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		if(Application.loadedLevelName == "BedRoom")
		{
			Destroy (gameObject , 0.5f);
		}
	}
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles = new Vector3(0,0,transform.eulerAngles.z+(Time.deltaTime*200));
	}
	
	void DisableIt()
	{
		gameObject.SetActive (false);
	}
}
