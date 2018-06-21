using UnityEngine;
using System.Collections;


public class ScrollChild : MonoBehaviour {
	public UITexture myTexture;
	public UILabel myLabel;
	public UIButton launchUrlButton;
	public ScrollChildVariables []scrollChildren = new ScrollChildVariables[3];


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class ScrollChildVariables
{
	public UITexture myTexture;
	public UILabel myLabel;
	public UIButton launchUrlButton;
	public GameObject myObject;
}