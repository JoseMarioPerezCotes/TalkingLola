using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision coll)
    {//void OnCollisionEnter(Collision coll) {
        //Debug.Log("collision ==========");
        coll.rigidbody.velocity = new Vector3(0, 0, 0);
        if (coll.rigidbody.velocity.y <= 0)
		{
			coll.rigidbody.velocity = new Vector3(0,1,0);
			coll.rigidbody.AddForce (0,600,0);
			coll.transform.GetComponent<Animation>().Play ("Jump");
           

            if (Application.loadedLevelName != "VirtualReality")
			{
				GetComponent<Animation>()["TrampolineAnim"].speed = 2.5f;
				GetComponent<Animation>().Play ();
				AnimationHandler._instance.isJumping = true;
				if(PlayerPrefs.GetInt ("Sound") == 1)
				{
					AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.jump;
					AnimationHandler._instance.GetComponent<AudioSource>().Play ();
				}
			}
			else
			{
				VirtualAnimationHandler._instance.isJumping = true;
			}
		}
	}



}
