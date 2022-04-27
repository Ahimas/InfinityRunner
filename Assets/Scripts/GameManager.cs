using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float speed = 10f;
    public int maxScores = 10;
    public int minScores = 0;

    public bool isGameActive = false;
    public float spawnInterval = 3f;
    public int scores;

    public Transform[] spawnPositions;

    [SerializeField] private Color[] gameColors = { Color.red, Color.blue, Color.green };
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Road road;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TextMeshProUGUI scoreCounter;

    private Renderer changeColorPlace;

    private bool isReadyForSpawn;

    // Start is called before the first frame update
    void Start()
    {
        changeColorPlace = GameObject.Find("ChangeColorPlace").GetComponent<Renderer>();
        spawnManager.SetColors(gameColors);
        spawnManager.PrepareCrystalPool((int)(20 / spawnInterval));

        startMenu.SetActive(true);

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

   public void UpdateScore(int num)
    {
        scores += num;
        scoreCounter.text = $"Scores: {scores}"; 

    }

    internal void UpdateRoad() {
        road.UpdateRoad();
        changeColorPlace.material.color = ChangeColor();

    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverMenu.SetActive(true);
    }

    public void StartGame()
    {
        SetRandomPlayerColor();
        UpdateRoad();

        scores = minScores;
        UpdateScore(minScores);

        startMenu.SetActive(false);
        scoreCounter.gameObject.SetActive(true);

        isGameActive = true;
        isReadyForSpawn = true;
    }

    public void RestartGame()
    {
        GameObject.Find("Player").transform.localScale = Vector3.one;
        GameObject.Find("Player").GetComponent<Animator>().SetBool("isGameOver", false);
        gameOverMenu.SetActive(false);
        spawnManager.DisableCrystalsInPool();
        StartGame();
    }

    void SetRandomPlayerColor()
    {
        GameObject.Find("Player").GetComponentInChildren<SkinnedMeshRenderer>().material.color = gameColors[Random.Range(0, gameColors.Length)];
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
