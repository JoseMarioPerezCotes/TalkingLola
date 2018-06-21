using UnityEngine;
using System.Collections;

public class LargeShapeTransition : MonoBehaviour
{
#region PUBLIC_VARS
		public float TimeLimit;
		public FADE_TYPE FadeType;
	
		public GameObject Shape;
	#endregion
	#region PRIVATE_VARS
		private float fadeInFact = 0.2f;//0.1f;
		private float initalScale = 3.0f;
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
				initalScale = (FadeType == FADE_TYPE.FADE_OUT) ? 0 : initalScale;
				if (TimeLimit == 0)
						TimeLimit = 10;
				if (FadeType == 0)
						FadeType = FADE_TYPE.FADE_IN;
		}
	
		void Update ()
		{
				TimeLimit = TimeLimit - Time.deltaTime;
				if (TimeLimit > 0) {
						if (FadeType == FADE_TYPE.FADE_IN && initalScale <= 0)
								initalScale = 0;
						else
								initalScale = initalScale - (10 / TimeLimit) * fadeInFact * Time.deltaTime * (int)FadeType;
						Shape.transform.localScale = new Vector3 (initalScale, 1, initalScale);
				} else
						Destroy (gameObject);
		}
		#endregion
}