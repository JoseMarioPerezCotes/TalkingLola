using UnityEngine;
using System.Collections;

public class AutoScroll : MonoBehaviour {
	// Use this for initialization
	bool keepOnScrolling;
	float gridWidth = 414f;
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
//				Debug.Log("move right = "+((loadedTextures-1)*gridWidth));
//				Debug.Log("transform.localPosition.x = "+transform.localPosition.x);
				if(transform.localPosition.x > -((loadedTextures-2)*gridWidth))
				{
					float Xpos = 0;
					if((transform.localPosition.x-gridWidth) < -((loadedTextures-2)*gridWidth))
						Xpos = -((loadedTextures-2)*gridWidth);
					else
						Xpos = transform.localPosition.x-gridWidth;

					SpringPanel.Begin (gameObject , new Vector3(Xpos,transform.localPosition.y,transform.localPosition.z) ,8);
				}
				else
				{
					moveRight = false;
				}
			}
			else
			{
//				Debug.Log("move left");
//				Debug.Log("transform.localPosition.x = "+transform.localPosition.x);
				float Xpos = 0;
				if((transform.localPosition.x+gridWidth) > 0)
					Xpos = 0;
				else
					Xpos = transform.localPosition.x+gridWidth;

				if(gameObject.transform.localPosition.x < -400f)
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
