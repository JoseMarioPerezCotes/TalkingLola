using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallShapeTransition : MonoBehaviour
{
#region PUBLIC_VARS
		public float TimeLimit;
		public FADE_TYPE FadeType;
		public Texture2D fadeTexture;
	#endregion
	
	#region PRIVATE_VARS	
		private float fadeInFact = 0.2f;//0.1f;
		private float initalScale = 5;
		private List<GameObject> cubeList;
		private float xPos, yPos;
		private int columns = 12, rows = 12;
		private float offset = 2.0f;
		private Vector3 ScreenSize;
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
				ScreenSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height));
				initalScale = (FadeType == FADE_TYPE.FADE_OUT) ? 0 : initalScale;
				initilaizeShapes ();
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
						foreach (GameObject cube in cubeList)
								cube.transform.localScale = new Vector3 (initalScale, initalScale, 1);
				} else
						Destroy (gameObject);
		}
#endregion
#region PRIVATE_METHODS
		private void initilaizeShapes ()
		{
				xPos = (ScreenSize.x * -1);
				yPos = (ScreenSize.y * -1);
				Material materialType = new Material (Shader.Find ("Transparent/Diffuse"));
				materialType.mainTexture = fadeTexture;
				cubeList = new List<GameObject> ();
				for (int i=0; i<columns; i++) {
						for (int j=0; j<rows; j++) {
								GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
								cube.GetComponent<Renderer>().material = materialType;
								cube.transform.localScale = new Vector3 (initalScale, initalScale, 1);
								cube.transform.parent = transform;
								cube.transform.position = new Vector3 (xPos, yPos, 0);
								cubeList.Add (cube);
								xPos += offset;
						}
						xPos = (ScreenSize.x * -1);// - offset;//(columns/2 -1) * offset * -1;
						yPos += offset;
				}
		}
#endregion
		
}