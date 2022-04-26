using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Touch theTouch;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    private GameManager gameManager;


 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if ( Input.touchCount > 0 )
        {
            theTouch = Input.GetTouch(0);

            if ( theTouch.phase == TouchPhase.Began )
            {
                startTouchPos = theTouch.position;
            } else if ( theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended )
            {
                endTouchPos = theTouch.position;
            }

            float x = endTouchPos.x - startTouchPos.x;
            float y = endTouchPos.y - startTouchPos.y;
            Vector3 direction = Vector3.left;

            if (Mathf.Abs(x) > Mathf.Abs(y)) direction = Vector3.right;

            transform.Translate(direction * gameManager.speed * Time.deltaTime);


        }
        
    }
}
