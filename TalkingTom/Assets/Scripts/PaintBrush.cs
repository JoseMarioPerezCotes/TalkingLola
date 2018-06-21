using UnityEngine;
using System.Collections;

public class PaintBrush : MonoBehaviour {


	bool isMoving;
	public GameObject pasteParent;
	public GameObject bubble; 
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown()
	{
		isMoving = true;
	}

	void OnMouseUp()
	{
		isMoving = false;
	}

	void OnCollisionEnter(Collision coll) {
		foreach (ContactPoint contact in coll.contacts) {

			if(coll.transform.name == "paste" && isMoving)
			{
				if(!coll.transform.GetComponent<SpriteRenderer>().enabled)
				{
					coll.transform.GetComponent<SpriteRenderer>().enabled = true;
					coll.transform.GetComponent<Collider>().enabled = false;
					if(coll.transform.childCount > 0)
					{
						SpriteRenderer []chileRenderer =  coll.transform.GetComponentsInChildren<SpriteRenderer>();
						foreach(SpriteRenderer a in chileRenderer)
						{
							a.enabled = true;

						}
					}
					if(!AnimationHandler._instance.isSoapOnBody)
						bubble.GetComponent<ParticleSystem>().Play ();
					AnimationHandler._instance.isSoapOnBody = true;

				}
			}
		}
	}

	void OnCollisionStay(Collision coll) {
		foreach (ContactPoint contact in coll.contacts) {

			if(coll.transform.name == "paste" && isMoving)
			{
				if(!coll.transform.GetComponent<SpriteRenderer>().enabled)
				{
					coll.transform.GetComponent<SpriteRenderer>().enabled = true;
					coll.transform.GetComponent<Collider>().enabled = false;
					if(!AnimationHandler._instance.isSoapOnBody)
						bubble.GetComponent<ParticleSystem>().Play ();
					AnimationHandler._instance.isSoapOnBody = true;
					if(coll.transform.childCount > 0)
					{
						SpriteRenderer []chileRenderer =  coll.transform.GetComponentsInChildren<SpriteRenderer>();
						foreach(SpriteRenderer a in chileRenderer)
						{
							a.enabled = true;
						}
					}
				}
			}
		}
	}
	
}
