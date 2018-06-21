using UnityEngine;
using System.Collections;

public class PasteRemoval : MonoBehaviour {
	// Use this for initialization
	public bool isTowel;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(other.gameObject);  // Remove pastes
//		if(isTowel && ObjectMotion.waterDroplets != null)
//		{
//			Destroy(ObjectMotion.waterDroplets);
//			if(!GameObject.Find("paste(Clone)")) // if no paste remains and water is cleared , bring star particles
//				Instantiate(Resources.Load("Cleared"));
//		}
	}
}
