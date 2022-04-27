using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float changeRowTime = 0.25f;
    public float scaleMultiplier = 1.1f;

    private Touch theTouch;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Vector2 rowLimits;
    private SkinnedMeshRenderer playerRenderer;

    private int currentRow;

    private bool isMoving;

    private GameManager gameManager;
    private Animator playerAnimator;

    enum Direction
    {
        Left = -1,
        Right = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerAnimator = GetComponent<Animator>();

        currentRow = (int)gameManager.spawnPositions.Length / 2;
        transform.position = new Vector3(gameManager.spawnPositions[currentRow].position.x, transform.position.y, transform.position.z);
        rowLimits = new Vector2(0, gameManager.spawnPositions.Length - 1);

        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( gameManager.isGameActive )
        {
            TouchController();
            KeyboardController();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "ChangeColorPlace")
        {
            playerRenderer.material.color = other.gameObject.GetComponent<Renderer>().material.color;
        }

        if ( other.gameObject.tag == "Crystal" )
        {
            if ( other.gameObject.GetComponentInChildren<MeshRenderer>().material.color != playerRenderer.material.color)
            {
                Decrease();
            } else
            {
                Increase();
            }
        }
    }

    void Increase()
    {
        if ( gameManager.scores < gameManager.maxScores )
        {
            gameManager.UpdateScore(+1);
            transform.localScale *= scaleMultiplier;
        }
        
    }

    void Decrease()
    {
        if ( gameManager.scores > gameManager.minScores )
        {
            transform.localScale /= scaleMultiplier;
            gameManager.UpdateScore(-1);
        } else
        {
            gameManager.GameOver();
            playerAnimator.SetBool("isGameOver", true);
        }
        
    }

    IEnumerator ChangeRow(int rowDir)
    {
        isMoving = true;
        currentRow += rowDir;

        Vector3 startPos = this.transform.position;
        Vector3 endPos = new Vector3(gameManager.spawnPositions[currentRow].position.x, transform.position.y, transform.position.z);

        float lerpDistance = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;
        float lenghtCovered = (Time.time - startTime) * changeRowTime;
        float partOfLenght = lenghtCovered / lerpDistance;

        while (partOfLenght < 1)
        {
            lenghtCovered = (Time.time - startTime) * changeRowTime;
            partOfLenght = lenghtCovered / lerpDistance;

            transform.position = Vector3.Lerp(startPos, endPos, partOfLenght);

            yield return null;
        }

        isMoving = false;

    }

    void TouchController()
    {
        if (Input.touchCount > 0 && !isMoving)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                startTouchPos = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                endTouchPos = theTouch.position;
            }

            float x = endTouchPos.x - startTouchPos.x;
            float y = endTouchPos.y - startTouchPos.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0 && currentRow != rowLimits.y)
                {
                    StartCoroutine(ChangeRow((int)Direction.Right));

                }
                else if (x < 0 && currentRow != rowLimits.x)
                {
                    StartCoroutine(ChangeRow((int)Direction.Left));
                }

            }

        }
    }

    void KeyboardController()
    {
        if ( Input.GetAxis("Horizontal") != 0 && !isMoving )
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if ( horizontalInput > 0 && currentRow != rowLimits.y )
            {
                StartCoroutine(ChangeRow((int)Direction.Right));

            } else if ( horizontalInput < 0 && currentRow != rowLimits.x )
            {
                StartCoroutine(ChangeRow((int)Direction.Left));

            }
        }
    }


}
