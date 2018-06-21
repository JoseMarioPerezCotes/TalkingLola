using UnityEngine;
using System.Collections;

public class LoaderPanel : MonoBehaviour {
	public UISprite myFillingSprite;
	public static bool loaderActive , asyncLoad;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable()
	{
		loaderActive = true;
		myFillingSprite.fillAmount = 0;
	}
	// Update is called once per frame
	void Update () {
		if(!asyncLoad)
		{
			myFillingSprite.fillAmount+=Time.deltaTime;
			if(myFillingSprite.fillAmount >= 0.998f)
			{
				myFillingSprite.fillAmount = 0;
			}
		}
	}

	void OnDisable()
	{
		loaderActive = false;
	}
}
