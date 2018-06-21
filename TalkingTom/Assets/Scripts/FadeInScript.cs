using UnityEngine;
using System.Collections;

public class FadeInScript : MonoBehaviour
{
#region PUBLIC_VARS
		public float TimeLimit;
		public FADE_TYPE FadeType;
	#endregion
	#region PRIVATE_VARS
		private float fadeInFact = 0.06f;//0.1f;
		private Color materialColor;
	#endregion
	#region ENUMERATION

		public enum FADE_TYPE
		{
				FADE_IN = 1,
				FADE_OUT = -1
		}
		#endregion
		#region UNITY_CALLBACKS
		void Start ()
		{
				if (TimeLimit == 0)
						TimeLimit = 10;
				if (FadeType == 0)
						FadeType = FADE_TYPE.FADE_IN;
				materialColor = GetComponent<Renderer>().material.color;
				materialColor.a = (FadeType == FADE_TYPE.FADE_IN) ? materialColor.a : 0;
		}
	
		void Update ()
		{
				TimeLimit = TimeLimit - Time.deltaTime;
				if (TimeLimit > 0) {
						materialColor.a = materialColor.a - (10 / TimeLimit) * fadeInFact * Time.deltaTime * (int)FadeType;
						GetComponent<Renderer>().material.color = materialColor;
				} else
						Destroy (gameObject);
		}
		#endregion
}