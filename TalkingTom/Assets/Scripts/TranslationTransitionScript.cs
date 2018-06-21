using UnityEngine;
using System.Collections;

public class TranslationTransitionScript : MonoBehaviour
{
#region PUBLIC_METHODS
		public float Speed = 2;
		public TRASLATION_DIRECTION TranslationDirection;
		public Sprite MaskTexture;
	#endregion
	
	#region PRIVATE_METHODS
		private Vector3 camPos;
		private Vector3 camInitialPos;
		private Vector3 ScreenSize;
		private GameObject mask;
	#endregion
	#region ENUMERATION
		public enum TRASLATION_DIRECTION
		{
				LEFT = 1,
				RIGHT = 2,
				UP = 3,
				DOWN = 4
		}
#endregion
#region UNITY_CALLBACKS
		void Start ()
		{
				camPos = Camera.main.transform.position;
				camInitialPos = Camera.main.transform.position;
				if (TranslationDirection == 0)
						TranslationDirection = TRASLATION_DIRECTION.LEFT;
				if (Speed == 0)
						Speed = 10;
				print ("Speed=" + Speed);
				ScreenSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height));
				ScreenSize = ScreenSize * 2;
				createMask ();
				setCameraPosition ();
		}
		void Update ()
		{
				if (TranslationDirection == TRASLATION_DIRECTION.LEFT)
						Camera.main.transform.Translate (Vector3.left * -Speed * Time.deltaTime);
				else if (TranslationDirection == TRASLATION_DIRECTION.RIGHT)
						Camera.main.transform.Translate (Vector3.right * -Speed * Time.deltaTime);
				else if (TranslationDirection == TRASLATION_DIRECTION.UP)
						Camera.main.transform.Translate (Vector3.up * -Speed * Time.deltaTime);
				else if (TranslationDirection == TRASLATION_DIRECTION.DOWN)
						Camera.main.transform.Translate (Vector3.down * -Speed * Time.deltaTime);
				//		Camera.main.transform.Translate(Vector3.left * -Speed * Time.deltaTime);
				if (TranslationDirection == TRASLATION_DIRECTION.LEFT && Camera.main.transform.position.x > camInitialPos.x)
						destroyGameObject ();
				else if (TranslationDirection == TRASLATION_DIRECTION.RIGHT && Camera.main.transform.position.x < camInitialPos.x)
						destroyGameObject ();
				else if (TranslationDirection == TRASLATION_DIRECTION.UP && Camera.main.transform.position.y < camInitialPos.y)
						destroyGameObject ();
				else if (TranslationDirection == TRASLATION_DIRECTION.DOWN && Camera.main.transform.position.y > camInitialPos.y)
						destroyGameObject ();
		}
#endregion
#region PRIVATE_METHODS
		private void createMask ()
		{
				mask = new GameObject ("Mask");
				SpriteRenderer spriteRenderer = mask.AddComponent<SpriteRenderer> ();
				if (MaskTexture != null) {
//			return;
						spriteRenderer.sprite = MaskTexture;
						mask.transform.localScale = new Vector3 (ScreenSize.x / spriteRenderer.bounds.size.x, ScreenSize.y / spriteRenderer.bounds.size.y, 1);
				}
				mask.transform.parent = transform;
		}

		private void setCameraPosition ()
		{
				if (TranslationDirection == TRASLATION_DIRECTION.LEFT) {
						mask.transform.position = new Vector3 (camPos.x - ScreenSize.x, camPos.y, camPos.z + 1);
						camPos.x = camPos.x - ScreenSize.x;
				} else if (TranslationDirection == TRASLATION_DIRECTION.RIGHT) {
						mask.transform.position = new Vector3 (camPos.x + ScreenSize.x, camPos.y, camPos.z + 1);
						camPos.x = camPos.x + ScreenSize.x;
				} else if (TranslationDirection == TRASLATION_DIRECTION.UP) {
						mask.transform.position = new Vector3 (camPos.x, camPos.y + ScreenSize.y, camPos.z + 1);
						camPos.y = camPos.y + ScreenSize.y;
				} else if (TranslationDirection == TRASLATION_DIRECTION.DOWN) {
						mask.transform.position = new Vector3 (camPos.x, camPos.y - ScreenSize.y, camPos.z + 1); 
						camPos.y = camPos.y - ScreenSize.y;
				}
				Camera.main.transform.position = camPos;

		}

		

		private void destroyGameObject ()
		{
				Camera.main.transform.position = camInitialPos;
				Destroy (gameObject);
		}
		#endregion
}