using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float speed = 10f;
    public bool isGameActive;
    public float spawnInterval = 3f;

    [SerializeField] private Color[] gameColors = { Color.red, Color.blue, Color.green };
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Road road;

    private Renderer changeColorPlace;

    private bool isReadyForSpawn;

    // Start is called before the first frame update
    void Start()
    {
        changeColorPlace = GameObject.Find("ChangeColorPlace").GetComponent<Renderer>();
        spawnManager.SetColors(gameColors);
        spawnManager.PrepareCrystalPool(gameColors.Length * gameColors.Length);
        
        UpdateRoad();

        isGameActive = true;
        isReadyForSpawn = true;

    }

    // Update is called once per frame
    void Update()
    {
        if ( isGameActive && isReadyForSpawn )
        {
            isReadyForSpawn = false;
            spawnManager.SpawnCrystalRow();

            Invoke("SpawnInterval", spawnInterval);
        }
    }

    internal void UpdateRoad() {
        road.UpdateRoad();
        changeColorPlace.material.color = ChangeColor();

    }

    private Color ChangeColor()
    {
        Color newColor = gameColors[Random.Range(0, gameColors.Length)];

        if (newColor == changeColorPlace.material.color) return ChangeColor();

        return newColor;
    }

    void SpawnInterval()
    {
        isReadyForSpawn = true;
    }
    

}
