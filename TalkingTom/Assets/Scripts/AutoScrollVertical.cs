using UnityEngine;
using System.Collections;

public class AutoScrollVertical : MonoBehaviour {

	// Use this for initialization
	bool keepOnScrolling;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	int loadedTextures;
	bool moveRight = true;
	void OnEnable()
	{
		keepOnScrolling = true;
		StartCoroutine("ScrollMe");
	
		loadedTextures = transform.GetComponentInChildren<UIGrid>().transform.childCount;
	}

	IEnumerator ScrollMe()
	{
		while(keepOnScrolling)
		{
			yield return new WaitForSeconds(3.0f);
			if(moveRight)
			{
				if(transform.localPosition.x > -((loadedTextures-1)*360f))
				{
					float Xpos = 0;
					if((transform.localPosition.x-360f) < -((loadedTextures-1)*360))
						Xpos = -((loadedTextures-1)*360);
					else
						Xpos = transform.localPosition.x-360f;

					SpringPanel.Begin (gameObject , new Vector3(Xpos,transform.localPosition.y,transform.localPosition.z) ,8);
				}
				else
				{
					moveRight = false;
				}
			}
			else
			{
				float Xpos = 0;
				if((transform.localPosition.x+360f) > 0)
					Xpos = 0;
				else
					Xpos = transform.localPosition.x+360f;

				if(gameObject.transform.localPosition.x < -300f)
					SpringPanel.Begin (gameObject , new Vector3(Xpos,transform.localPosition.y,transform.localPosition.z) ,8);
				else
				{
					moveRight = true;
				}
			}
		}
	}

	void OnDisable()
	{
		StopCoroutine("ScrollMe");
		keepOnScrolling = false;
	}
}
