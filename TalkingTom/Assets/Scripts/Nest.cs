using UnityEngine;
using System.Collections;

public class Nest : MonoBehaviour {

    #region PUBLIC_VARIABLES
    public bool isCameraPoint;

    public bool isNestGenerated;

    public bool isStaticNest;

    public DisplacementDirection direction;
    /// The amount of displacement.
    /// </summary>
    public float amountOfDisplacement = 2;

    /// <summary>
    /// The object is moving right.
    /// </summary>
    public bool isMovingRight;

    public float speed = 1f;
    #endregion

    #region PRIVATE_VARIABLES
    private float initialXComponent = 0;

    private Vector3 initialPosition;
    #endregion

    #region UNITY_CALLBACKS
    void Awake()
    {

        initialPosition = transform.localPosition;
        if (DisplacementDirection.Z == direction)
        {
            initialXComponent = transform.localPosition.z;
        }
        else
        {
            initialXComponent = transform.localPosition.x;
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        if (!isStaticNest)
        {
            float displacement;
            if (isMovingRight)
                displacement = Mathf.Sin(Time.time * speed) * -amountOfDisplacement;
            else
                displacement = Mathf.Sin(Time.time * speed) * amountOfDisplacement;
            if (direction == DisplacementDirection.Z)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, initialXComponent + displacement);
            else
                transform.localPosition = new Vector3(initialXComponent + displacement, transform.localPosition.y, transform.localPosition.z);

        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>().velocity.y<0)
        {
            other.GetComponent<SpriteRenderer>().sortingOrder = 1;
            other.GetComponent<Rigidbody2D>().isKinematic = true;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            other.transform.parent = transform;
            
            if(!isNestGenerated){
                isNestGenerated = true;
                GameManager.instance.GenerateNewNest();
            }
           if (isCameraPoint) MainCamera.instance.MoveCameraUp(transform.position.y+5.5f);
           StartCoroutine(BirdAnimation(0.25f));
        }
    }


    #endregion

    #region PRIVATE_METHODS
    private IEnumerator BirdAnimation(float time)
    {
        SoundManager.instance.PlayBirdLand();
        Vector3 fromPos = Bird.instance.transform.localPosition;
        Vector3 targetPos = new Vector3(0, 0.8f, 0);
        float i = 0;
        float rate = 1 / time;
        while (i < 1)
        {
            i += rate * Time.deltaTime;
            Vector3 pos = Vector3.Lerp(fromPos, targetPos, i);
            Bird.instance.transform.localPosition = pos;
            yield return 0;
        }
        Bird.instance.isBirdMoving = false;
    }
    #endregion

    #region PUBLIC_METHODS
    #endregion

    #region COROUTINES
    #endregion

}


