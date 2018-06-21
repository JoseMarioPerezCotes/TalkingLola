using UnityEngine;
using System.Collections;

public class MyCounter : MonoBehaviour {
	public static int counter;
	TextMesh myMesh;
	bool startCounter,stopCounter;
	float timer;
	// Use this for initialization
	void Start () {
		myMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if(startCounter)
		{
			timer+=Time.deltaTime;
			if(timer >= (0.5f*(1/AnimationHandler._instance.runSpeed)))
			{
				timer = 0;
				IncrementCounter();
			}
		}
	}
	/// <summary>
	/// Stops the counter.
	/// </summary>
	public void StopCounter()
	{
		startCounter = false;
	}

	/// <summary>
	/// Starts the counter.
	/// </summary>
	public void StartCounter()
	{
		counter = 0;
		if(myMesh)
			myMesh.text = counter.ToString ();
		startCounter = true;
	}
	/// <summary>
	/// Increments the counter.
	/// </summary>
	void IncrementCounter()
	{
		counter++;
		myMesh.text = counter.ToString ();
	}

}
