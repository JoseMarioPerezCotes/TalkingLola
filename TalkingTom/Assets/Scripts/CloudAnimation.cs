using UnityEngine;
using System.Collections;

public class CloudAnimation : MonoBehaviour
{

	#region PUBLIC_VARS
		public Transform targetCloud;
    #endregion
    #region PRIVATE_VARS
		private Vector3 targetVector = new Vector3 (10, 8, 0);
    #endregion
    
    #region UNITY_CALLBACKS
		// Update is called once per frame
		void Update ()
		{
				if ((targetVector - transform.localPosition).magnitude > 0.5f)
						transform.localPosition += (targetVector - transform.localPosition).normalized * Time.deltaTime * 2;
				else
						transform.localPosition = new Vector3 (targetCloud.localPosition.x - 20, 8, 0);
		}
	#endregion
}
