using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private Color[] gameColors;

    private List<GameObject> crystalPool;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void PrepareCrystalPool(int requiredQnty)
    {
        GameObject crystal;

        crystalPool = new List<GameObject>();

        for ( int i = 0; i < requiredQnty; i++)
        {
            crystal = Instantiate(crystalPrefab);
            crystalPool.Add(crystal);
            crystal.SetActive(false);
        }
    }

    internal void SpawnCrystalRow()
    {
        gameColors = ShuffleColors(gameColors);

        for ( int i = 0; i < spawnPositions.Length; i++ )
        {
            GameObject crystal = GetCrystal();

            crystal.transform.position = spawnPositions[i].position;
            crystal.GetComponentInChildren<MeshRenderer>().material.color = gameColors[i];
            crystal.SetActive(true);
        }

    }

    GameObject GetCrystal()
    {
        foreach ( GameObject crystal in crystalPool )
        {
            if ( !crystal.activeSelf )
            {
                return crystal;
            }
        }

        return GetCrystal();
    }

    internal void SetColors(Color[] colors)
    {
        gameColors = colors;
    }

    private Color[] ShuffleColors(Color[] colors)
    {
        Color[] newColors = colors.Clone() as Color[];

        for (int i = 0; i < newColors.Length; i++)
        {
            Color tmp = newColors[i];
            int r = Random.Range(i, newColors.Length);

            newColors[i] = newColors[r];
            newColors[r] = tmp;
        }

        return newColors;

    }


}
