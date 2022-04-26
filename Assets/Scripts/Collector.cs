using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Crystal" ) other.gameObject.SetActive(false);
        if ( other.gameObject.tag == "ChangeColorPlace" ) gameManager.UpdateRoad();
    }

}
