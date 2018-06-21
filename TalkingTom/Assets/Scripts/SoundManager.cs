using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

	#region STATIC_VARS
		public static SoundManager instance;
    #endregion
    #region PUBLIC_VARS

		public AudioSource bgMusic;
		public AudioSource birdFly;
		public AudioSource jump;
		public AudioSource winState;

		public tk2dUIToggleButton togglebutton;
		public bool isSoundOn;
#endregion

#region UNITY_CALLBACKS
		// Awake is called when the script instance is being loaded
		void Awake ()
		{
				instance = this;
				SetInitSoundSetting ();
		}
#endregion
#region PUBLIC_METHODS
		public void SetInitSoundSetting ()
		{
				if (PlayerPrefs.HasKey ("Sound") && PlayerPrefs.GetInt ("Sound") == 0) {
						isSoundOn = false;
				} else {
						isSoundOn = true;
						PlayBackgroudMusic ();
				}
		}

//		public void SetSound ()
//		{
//				if (togglebutton.IsOn) {
//						isSoundOn = true;
//            
//						PlayerPrefs.SetInt ("Sound", 1);
//						PlayBackgroudMusic ();
//				} else {
//						DeactiveAll ();
//						isSoundOn = false;
//						PlayerPrefs.SetInt ("Sound", 0);
//				}
//		}

   

		public void DeactiveAll ()
		{
				bgMusic.Stop ();
				birdFly.Stop ();
//				jump.Stop ();
				winState.Stop ();
		}

		public void PlayBackgroudMusic ()
		{
				if (!isSoundOn)
						return;
				DeactiveAll ();
				if (!bgMusic.isPlaying)
						bgMusic.Play ();
		}

		public void PlayLevelComplete ()
		{
				if (!isSoundOn)
						return;
				winState.Play ();
		}


		public void PlayBirdFly ()
		{
				if (!isSoundOn)
						return;
				birdFly.Play ();
		}

		public void PlayBirdLand ()
		{
				if (!isSoundOn)
						return;
				birdFly.Stop ();
//				jump.Play ();
		}
		#endregion
}
