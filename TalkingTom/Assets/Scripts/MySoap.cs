using UnityEngine;
using System.Collections;

public class MySoap : MonoBehaviour {
	public SpriteRenderer []mySoap;
	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "BathRoom")
		{
			mySoap = transform.GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer a in mySoap)
			{
				CharacterSoap._instance.mySoap.Add(a);
				a.gameObject.AddComponent<Paste>();
				a.GetComponent<Paste>().myCollider = a.GetComponent<BoxCollider>();
				if(a.enabled)
				{
					a.enabled = false;
					if(a.transform.childCount > 0)
					{
						SpriteRenderer []chileRenderer =  a.transform.GetComponentsInChildren<SpriteRenderer>();
						foreach(SpriteRenderer b in chileRenderer)
						{
							b.enabled = false;
						}
					}
				}
			}
		}
		else
			Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
