using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VoiceRecognitionRepeating : MonoBehaviour {

	private bool dontRecord;
	private float notTouchTime;
	private int myTimeSamples;
	private float averageVolumeBuffer;
	private const float THRESHOLD = 0.02f; 
	private float rmsValue;            // Volume in RMS
	private float dbValue;             // Volume in DB
	private float pitchValue;  
	private float[] spectrum ;  
	int samplerate = 44100;

	public int clamp = 160;
	public AudioClip myClip;
	public float volume,lastVolume;
	public string idleAnim,talkAnim;
	public string []myTalkAnimations;
	public float timerSinceMicrophoneStarted;

	private static float REFVALUE  = 0.1f;  
	private static int SAMPLECOUNT = 1024;   // Sample Count.

	void Start() {
		Application.targetFrameRate = 60;
#if UNITY_IPHONE
		averageVolumeBuffer = 0.003f;
//		averageVolumeBuffer = 0.001f;
#elif UNITY_ANDROID
		averageVolumeBuffer = 0.003f;
//		averageVolumeBuffer = 0.001f;
#endif
		spectrum = new float[8192];
//		GetComponent<AudioSource>().mute = true; // Mute the sound, we don't want the player to hear it 
		if(Application.loadedLevelName != "BedRoom")
		{
			Invoke("Record",0.3f);
		}
	}



	/// <summary>
	/// Stops the microphone
	/// </summary>
	public void StopIt()
	{
//		Debug.Log("stop");
//		Microphone.End (null);
		checkingWhenToStop = false;
		StopCoroutine("CheckWhenToStop");
//		CancelInvoke("MuteIt");
		StopCoroutine ("WaitingForTalkToStop");
//		audio.Stop ();
	}

	void OnGUI() 
	{
//		GUI.Label(new Rect(100,200,100,100),"freq = "+GetFundamentalFrequency ());
//		GUI.Label(new Rect(200,200,100,100),"Last Samples Vol = "+LastSamplesVolume());
//		GUI.Label(new Rect(300,200,100,100),"timesince last rec = "+timerSinceMicrophoneStarted);
	}

	void Update()
	{
		timerSinceMicrophoneStarted+=Time.deltaTime;
		if(timerSinceMicrophoneStarted > 51f && !dontRecord && !AnimationHandler._instance.suspendActions && !TvAlphabets.tvOn && !AnimationHandler._instance.isPlayingSlapAnim && !AnimationHandler._instance.coinsPanelActive && !PopupPanel.popupPanelActive && !LoaderPanel.loaderActive && !GetComponent<Animation>().IsPlaying (talkAnim))
		{
			Debug.Log("record again after 50 sec");
			Record();
		}


		#if UNITY_EDITOR
		if(Input.GetMouseButton (0) || Input.GetMouseButtonUp (0) || Input.GetMouseButtonDown (0))
		{
			if(!dontRecord)
			{
//				GetComponent<AudioSource>().mute = true;
				if(AnimationHandler._instance != null && AnimationHandler._instance.audioPlayingSource != null)
					AnimationHandler._instance.audioPlayingSource.mute = true;
				if(GetComponent<Animation>().IsPlaying(talkAnim))
				{
					GetComponent<Animation>().Stop (talkAnim);
					StopCoroutine ("WaitingForTalkToStop");
				}
				if(GetComponent<Animation>().IsPlaying("Listen"))
				{
					StopListen();
				}
//				Microphone.End (null);
				checkingWhenToStop = false;
				StopCoroutine("CheckWhenToStop");
				notTouchTime = 0;

			}
			dontRecord = true;
		}
		else
		{
			if(dontRecord)
			{
				notTouchTime+=Time.deltaTime;
				if(notTouchTime > 1.0f)
				{
					if((!GetComponent<Animation>().isPlaying || (GetComponent<Animation>().isPlaying && GetComponent<Animation>().IsPlaying(idleAnim))) && !AnimationHandler._instance.suspendActions && !TvAlphabets.tvOn && !AnimationHandler._instance.GetComponent<AudioSource>().isPlaying && !ExtraInteractions.stopSoundRecordingDueToFlush)
					{
						SubsequentRecord();
						dontRecord = false;
					}

				}
				
			}
		}
		#else
		if(Input.touchCount > 0)
		{
			if(!dontRecord)
			{
//				audio.mute = true;
				AnimationHandler._instance.audioPlayingSource.mute = true;
				if(GetComponent<Animation>().IsPlaying(talkAnim))
				{
					GetComponent<Animation>().Stop (talkAnim);
					StopCoroutine ("WaitingForTalkToStop");
				}
				if(GetComponent<Animation>().IsPlaying("Listen"))
				{
					StopListen();
				}
				checkingWhenToStop = false;
				StopCoroutine("CheckWhenToStop");
			}
			dontRecord = true;
			notTouchTime = 0;
		}
		else
		{
			if(dontRecord)
			{
				notTouchTime+=Time.deltaTime;
				if(notTouchTime > 1.0f)
				{
					if((!GetComponent<Animation>().isPlaying || (GetComponent<Animation>().isPlaying && GetComponent<Animation>().IsPlaying(idleAnim))) && !AnimationHandler._instance.suspendActions && !TvAlphabets.tvOn && !AnimationHandler._instance.GetComponent<AudioSource>().isPlaying && !ExtraInteractions.stopSoundRecordingDueToFlush)
					{
						SubsequentRecord();
						dontRecord = false;
					}
					
				}
				
			}
		}
#endif
	}
	

	public void RecordAgain()
	{

		SoundAvailabilityDetection.isPlaying = true;
		StartCoroutine("CheckWhenToStop");

	}

	int  minFreq,maxFreq;
	/// <summary>
	/// Record this audio.
	/// </summary>
	public void Record()
	{
		GetComponent<AudioSource>().timeSamples = 0;
		timerSinceMicrophoneStarted = 0;
		myTimeSamples = 0;
		lastSample = 0;
//		audio.pitch = 1.0f;
//		Debug.Log("record");
		Microphone.GetDeviceCaps(null,  out minFreq, out  maxFreq);
		if ((minFreq + maxFreq) == 0)//These 2 lines of code are mainly for windows computers
			maxFreq = 44100;
		GetComponent<AudioSource>().clip = Microphone.Start(null, false, 50, maxFreq);
		iPhoneSpeaker.ForceToSpeaker();
		while (!(Microphone.GetPosition(null) > 0) )
		{
		}  

//		Debug.Log("minFreq = "+minFreq + "   maxFreq = "+maxFreq);
		iPhoneSpeaker.ForceToSpeaker();
		GetComponent<AudioSource>().Play();
//		GetComponent<AudioSource>().mute = true;
		SoundAvailabilityDetection.isPlaying = true;
		StartCoroutine("CheckWhenToStop");
	}


	public void SubsequentRecord()
	{
       // GetComponent<AudioSource>().pitch = 1.25f;
		dontRecord = false;
//		Debug.Log("subsequent record");
//		GetComponent<AudioSource>().mute = true;
		SoundAvailabilityDetection.isPlaying = true;
		StartCoroutine("CheckWhenToStop");
	}

	void StopListen()
	{
		if(GetComponent<Animation>().IsPlaying ("Listen"))
			GetComponent<Animation>().Play(idleAnim);
	}

	bool checkingWhenToStop;
	int finalSample;
	int lastSample;
	/// <summary>
	/// Checks  when the audio should stop.
	/// </summary>
	/// <returns>The when to stop.</returns>
	IEnumerator CheckWhenToStop()
	{
		if(!checkingWhenToStop)
		{
			#if UNITY_IPHONE
			averageVolumeBuffer = 0.003f;
//			averageVolumeBuffer = 0.001f;
			#elif UNITY_ANDROID
			averageVolumeBuffer = 0.003f;
//			averageVolumeBuffer = 0.001f;
			#endif
			checkingWhenToStop = true;
			float timer = 0.0f;
			float clipTime = 0.01f;
			int consecutiveNullVoice = 0;
			bool onceZero = false;
			bool gotASound = false;
			bool checkingThatSoundOnceGot = false;
			float myFreq = 0;
			myTimeSamples = GetComponent<AudioSource>().timeSamples;
			lastSample = GetComponent<AudioSource>().timeSamples;;
			while(consecutiveNullVoice < 11)
			{
				while( (GetAveragedVolume() > averageVolumeBuffer) && !AnimationHandler._instance.GetComponent<AudioSource>().isPlaying && !ExtraInteractions.stopSoundRecordingDueToFlush)
				{
					consecutiveNullVoice = 0;
					if(!gotASound && !ObjectMotion.bathroomObjectsDragged )
					{
						checkingThatSoundOnceGot = true;
//						Debug.Log("got sound");
						if( !AnimationHandler._instance.suspendActions && !TvAlphabets.tvOn && !AnimationHandler._instance.isPlayingSlapAnim && !AnimationHandler._instance.coinsPanelActive  && !PopupPanel.popupPanelActive && !LoaderPanel.loaderActive)
						{
							GetComponent<Animation>().Play("Listen");
							GetComponent<Animation>()["Listen"].speed = 0.3f;
						}
						clipTime = 0.01f;
//						Invoke("StopListen",transform.GetComponent<DetectActions>().listenAnim.length);
					}
					while(timer < 0.08f)
					{
						timer+=0.02f;
						clipTime+=0.02f;
						yield return 0;
					}
					finalSample = GetComponent<AudioSource>().timeSamples;
					gotASound = true;
					onceZero = false;
					timer = 0.0f;

					yield return 0;
				}
				if(!gotASound)
				{
					lastSample = myTimeSamples;
					myTimeSamples = GetComponent<AudioSource>().timeSamples;
				}
				if(onceZero)
				{
					float timerTocheck = 0.08f;
					checkingThatSoundOnceGot = false;
					consecutiveNullVoice++;
					timer = 0.0f;
					while(timer < timerTocheck)
					{
						timer+=0.02f;
						clipTime+=0.02f;
						yield return 0;
					}
					timer = 0.0f;
				}

				onceZero = true;

				
	//			Debug.Log("consecutiveNullVoice = "+consecutiveNullVoice);
				yield return 0;

			}
			checkingWhenToStop = false;
//			CancelInvoke("StopListen");
			if(!dontRecord && !ObjectMotion.bathroomObjectsDragged)
			{
				if(clipTime > 0.905f && !AnimationHandler._instance.GetComponent<AudioSource>().isPlaying && !TvAlphabets.tvOn && !AnimationHandler._instance.suspendActions && !AnimationHandler._instance.isPlayingSlapAnim && !AnimationHandler._instance.coinsPanelActive  && !PopupPanel.popupPanelActive && !LoaderPanel.loaderActive && gotASound)
				{
					finalSample = GetComponent<AudioSource>().timeSamples;
//					Debug.Log("final timesamples = "+finalSample);
					PlaySound (clipTime);
				}
				else
				{
					GetComponent<Animation>().Play("Idle");
					SubsequentRecord();
				}
			}

		}
	}


	/// <summary>
	/// Plays the sound.
	/// </summary>
	/// <param name="clipTime">Clip time.</param>
	public void PlaySound(float clipTime)
	{
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().clip = GetComponent<AudioSource>().clip;
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().timeSamples = lastSample;
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().pitch = 1.6f;
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().mute = false;
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().Play ();
//		audio.timeSamples = myTimeSamples;
		talkAnim = myTalkAnimations[Random.Range (0,myTalkAnimations.Length)];
		GetComponent<Animation>().Play (talkAnim);
//		audio.pitch = 1.4f;
//		audio.mute = false;
//		audio.Play ();
//		Debug.Log("Clip Length = "+clipTime);
//		CancelInvoke("MuteIt");
//		Invoke("MuteIt",clipTime/1.4f);
//		myTimeSamples = 0;

		StartCoroutine("WaitingForTalkToStop");
	}

	IEnumerator WaitingForTalkToStop()
	{
		while(AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().timeSamples < (finalSample-5000))
		{
			yield return 0;
		}
		MuteIt();
	}

	/// <summary>
	/// Stops and mutes the audisource
	/// </summary>
	void MuteIt()
	{
//		Debug.Log("mute");
//		audio.Stop ();
		Debug.Log("timesamples = "+AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().timeSamples);
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().Stop ();
		AnimationHandler._instance.audioPlayingSource.GetComponent<AudioSource>().clip = null;
//		AnimationHandler._instance.audioPlayingSource.audio.mute = true;
//		GetComponent<AudioSource>().mute = true;
		SoundAvailabilityDetection.isPlaying = false;
		GetComponent<Animation>().Play (idleAnim);
//		RecordAgain();
//		audio.timeSamples = 0;
		Invoke("RecordAgain",0.4f); 
	}

	void OnApplicationPause(bool isPaused)
	{
		Screen.orientation = ScreenOrientation.Portrait;
		Debug.Log ("isPaused = "+isPaused);
		if(isPaused)
		{
//			Debug.Log("stop recording");
			StopIt ();
		}
		else
		{
//			Debug.Log("start recording");
			if(Application.loadedLevelName != "BedRoom")
				Record ();
		}
	}

	/// <summary>
	/// Changes the data.
	/// </summary>
	void ChangeData()
	{
		float[] samples = new float[GetComponent<AudioSource>().clip.samples * GetComponent<AudioSource>().clip.channels];
		GetComponent<AudioSource>().clip.GetData(samples, 0);
		int i = 0;
		while (i < samples.Length) {
			samples[i] = samples[i] * 1.5F;
			++i;
		}
		GetComponent<AudioSource>().clip.SetData(samples, 0);
	}


	/// <summary>
	/// Volume of the last samples
	/// </summary>
	/// <returns>The last samples volume.</returns>
	float LastSamplesVolume()
	{ 
//		float[] data = new float[512];
//		float a = 0;
//		audio.GetOutputData(data,0);
//		for(int i = 502;i< 512;i++)
//		{
//			a += Mathf.Abs(data[i]);
//		}
//		lastVolume = a/10;
//		return a/10;
		float[] data = new float[512];
		float a = 0;
		GetComponent<AudioSource>().GetOutputData(data,0);
		
		for(int i = 511;i< 512;i++)
		{
			a += Mathf.Abs(data[i]);
		}
		volume = (a/1);
		return a/1;
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
	/// Analyzes the sound - find pitch
	/// </summary>
	void AnalyzeSound()
	{
		// Get all of our samples from the mic.
		float []samples = new float[SAMPLECOUNT];
		GetComponent<AudioSource>().GetOutputData(samples, 0);
		// Sums squared samples
		float sum =  0;
		for (int i = 0; i < SAMPLECOUNT; i++) {
			sum += Mathf.Pow(samples[i], 2);
		}
		
		// RMS is the square root of the average value of the samples.
		// Used to calculate the volume in dB
		rmsValue = Mathf.Sqrt (sum / SAMPLECOUNT);
		dbValue = 20 * Mathf.Log10 (rmsValue / REFVALUE);
		
		// Clamp it to {clamp} min
		if (dbValue < -clamp) {
			dbValue = -clamp;
		}
		
		// Gets the sound spectrum.
		GetComponent<AudioSource>().GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);
		float maxV = 0;
		int maxN = 0;
		
		// Find the highest sample.
		for (int j = 0; j < SAMPLECOUNT; j++) {
			if (spectrum [j] > maxV && spectrum [j] > THRESHOLD) {
				maxV = spectrum [j];
				maxN = j; // maxN is the index of max
			}
		}
		
		// Pass the index to a float variable
		float freqN = maxN;
		
		// Interpolate index using neighbours
		if (maxN > 0 && maxN < SAMPLECOUNT - 1) {
			float dL = spectrum [maxN - 1] / spectrum [maxN];
			float dR = spectrum [maxN + 1] / spectrum [maxN];
			freqN += 0.5f * (dR * dR - dL * dL);
		}
		
		// Convert index to frequency
		pitchValue = freqN * 24000 / SAMPLECOUNT;
		Debug.Log("pitch = "+pitchValue);
	}

	float GetFundamentalFrequency()
	{
		float fundamentalFrequency = 0.0f;
		float[] data = new float[8192];
		GetComponent<AudioSource>().GetSpectrumData(data,0,FFTWindow.BlackmanHarris);
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j < 8192; j++)
		{
			if ( s < data[j] )
			{
				s = data[j];
				i = j;
			}
		}
		fundamentalFrequency = i * samplerate / 8192;
		return fundamentalFrequency;
	}


}
