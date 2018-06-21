using UnityEngine;
using System.Collections;



public class MainCamera : MonoBehaviour
{

    #region PUBLIC_VARIABLES
    public static MainCamera instance;

    public Transform bird;
    #endregion

    #region PRIVATE_VARIABLES
    #endregion

    #region UNITY_CALLBACKS
    // Awake is called when the script instance is being loaded
    void Awake()
    {
        instance = this;
    }


    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        float y = Mathf.Clamp(bird.position.y,0,Mathf.Infinity);
     //   transform.position = Vector3.Lerp(transform.position,new Vector3(0,y,-10),Time.deltaTime*3);
    }


    #endregion

    #region PRIVATE_METHODS
    private IEnumerator MoveCamera(Vector3 fromPos,Vector3 targetPos,float time) {
        float i = 0;
        float rate = 1 / time;
        while(i<1){
            i += rate * Time.deltaTime;
            Vector3 pos = Vector3.Lerp(fromPos,targetPos,i);
            transform.position = pos;
            yield return 0;
        }

    }
    #endregion

    #region PUBLIC_METHODS
    public void MoveCameraUp(float y) {
        Vector3 targetPos = transform.position;
        targetPos.y = y;
        StartCoroutine(MoveCamera(transform.position,targetPos,1));
    }
    #endregion

    #region COROUTINES
    #endregion

}
