using UnityEngine;
using System.Collections;

public class CloudMotion : MonoBehaviour {
	public float myLocalPositionX;
	public bool right;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(VirtualRealityPanel.allowCloudMotion)
		{
			if(right)
			{
				transform.localPosition = new Vector3(transform.localPosition.x + Time.deltaTime*2 , transform.localPosition.y,transform.localPosition.z);
				if(transform.localPosition.x > myLocalPositionX+5f)
				{
					right = false;
				}
			}
			else
			{
				transform.localPosition = new Vector3(transform.localPosition.x - Time.deltaTime*2 , transform.localPosition.y,transform.localPosition.z);
				if(transform.localPosition.x < myLocalPositionX-5f)
				{
					right = true;
				}
			}
		}
	}
}
