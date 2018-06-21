using UnityEngine;
using System.Collections;

public class SoundAvailabilityDetection : MonoBehaviour {
	public float sensitivity = 100;
	public float loudness = 0;
	public float averageVolumeBuffer = 0.1f;
	public static bool isPlaying;
	public float volume;
	public float myFundamentalFrequency;
	int samplerate = 11024;
	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		averageVolumeBuffer = 0.08f;
#else
		averageVolumeBuffer = 0.003f;
#endif
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		GetComponent<AudioSource>().clip = Microphone.Start(null, true, 100, 44100);
		GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
		GetComponent<AudioSource>().mute = true; // Mute the sound, we don't want the player to hear it
//		while (!(Microphone.GetPosition(AudioInputDevice) > 0)){} // Wait until the recording has started
		while (!(Microphone.GetPosition(null) > 0) ){} // Wait until the recording has started
			GetComponent<AudioSource>().Play(); // Play the audio source!
		Debug.Log("playing");
	}
	
	// Update is called once per frame

	void Update(){
		loudness = GetAveragedVolume() * sensitivity;
		if( GetAveragedVolume() > averageVolumeBuffer && GetFundamentalFrequency() >= 30.0f && GetFundamentalFrequency() <= 3400.0f )  
		{
			if(!isPlaying)
			{
				GameObject.Find("Cube").GetComponent<VoiceRecognition>().Record();
			}
		}
	}

	/// <summary>
	///Start camera recording again
	/// </summary>
	public void RecordAgain()
	{
		Debug.Log("record again");
		GetComponent<AudioSource>().clip = Microphone.Start(null, true, 100, 44100);
		while (!(Microphone.GetPosition(null) > 0) ){}
		GetComponent<AudioSource>().Play ();
		GetComponent<AudioSource>().mute = true;
	}

	/// <summary>
	/// Gets the averaged volume.
	/// </summary>
	/// <returns>The averaged volume.</returns>
	float GetAveragedVolume()
	{ 
		float[] data = new float[256];
		float a = 0;
		GetComponent<AudioSource>().GetOutputData(data,0);
		
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		volume = (a/256);
		return a/256;

	}

	/// <summary>
	/// Gets the fundamental frequency.
	/// </summary>
	/// <returns>The fundamental frequency.</returns>
	float GetFundamentalFrequency()
	{
		float fundamentalFrequency = 0.0f;
		float []data = new float[8192];
		GetComponent<AudioSource>().GetSpectrumData(data,0,FFTWindow.BlackmanHarris);
		float s = 0.0f;
		int i = 0;
		
		for ( int j = 1; j < 8192; j++)
		{
			
			if ( s < data[j] )
			{
				s = data[j];
				i = j;
			}
		}
		fundamentalFrequency = i * samplerate / 8192;
		myFundamentalFrequency = fundamentalFrequency;
		return fundamentalFrequency;
	}
}
