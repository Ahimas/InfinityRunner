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
    private GameObject player;

    private bool isReadyForSpawn;

    // Start is called before the first frame update
    void Start()
    {
        changeColorPlace = GameObject.Find("ChangeColorPlace").GetComponent<Renderer>();
        spawnManager.SetColors(gameColors);
        spawnManager.PrepareCrystalPool((int)(20 / spawnInterval));
        player = GameObject.Find("Player");
        

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
        UpdateRoad();
        SetRandomPlayerColor();

        scores = minScores;
        UpdateScore(minScores);

        startMenu.SetActive(false);
        scoreCounter.gameObject.SetActive(true);

        isGameActive = true;
        isReadyForSpawn = true;
    }

    public void RestartGame()
    {
        player.transform.localScale = Vector3.one;
        player.GetComponent<Animator>().SetBool("isGameOver", false);
        gameOverMenu.SetActive(false);
        spawnManager.DisableCrystalsInPool();
        StartGame();
    }

    void SetRandomPlayerColor()
    {
        player.GetComponentInChildren<SkinnedMeshRenderer>().material.color = ChangeColor();
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
