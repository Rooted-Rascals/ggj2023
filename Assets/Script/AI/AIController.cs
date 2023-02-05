using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    
    
    private static AIController INSTANCE;
    public static AIController Instance
    {
        get
        {
            return INSTANCE;
        }
    }
    
    private static float TARGET_RADIUS = 0.1f;
    [SerializeField]
    AISpawner aiSpawner;

    private List<AISpawner> aiSpawners = new List<AISpawner>();
    private List<AI> aiList = new List<AI>();
    private Dictionary<AI, List<Vector3>> aiPaths = new Dictionary<AI, List<Vector3>>();

    [SerializeField]
    private float spawnDelay = 20f;
    [SerializeField]
    private int spawnerCount = 3;

    [SerializeField] private long maxAmountOfAi = 5;

    [SerializeField] private AIPathManager aiPathManager;
    [SerializeField] private float timeBeforeFirstSpawn = 30f;
    
    private float delay = 0f;

    public void UpdateAIGrid()
    {
        aiPathManager.UpdateAIGrid();
        UpdateSpawnPoints();
    }
    void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else if (INSTANCE != this)
        {
            Destroy(this);
        }
    }
    
    void Start()
    {
        if (aiPathManager == null)
        {
            aiPathManager = GetComponent<AIPathManager>();
        }
        UpdateAIGrid();
    }

    void Update()
    {
        timeBeforeFirstSpawn -= Time.deltaTime;

        if (timeBeforeFirstSpawn > 0)
        {
            return;
        } 
        
        UpdateAIs();
        SpawnAIs();
    }

    private void SpawnAIs()
    {
        delay += Time.deltaTime;
        if (delay >= spawnDelay && aiList.Count < maxAmountOfAi)
        {
            delay = 0;
            if (aiSpawners.Count <= 0)
            {
                return;
            }

            AISpawner spawner = aiSpawners[Random.Range(0, aiSpawners.Count)];
            AI spawnedAi = spawner.SpawnAI();
            HealthManager aiHealth = spawnedAi.GetComponent<HealthManager>();
            aiHealth.onDeath.AddListener(() =>
            {
                aiList.Remove(spawnedAi);
                aiPaths.Remove(spawnedAi);
                spawnedAi.Kill();
            });
            aiList.Add(spawnedAi);
            aiPaths.Add(spawnedAi, spawner.GetAIPath());
        }
        
    }

    private void UpdateSpawnPoints()
    {
        List<Tile> spawnPossibleTiles = aiPathManager.GetSpawnPossibleTiles();
        List<Tile> randomizedTiles = spawnPossibleTiles.OrderBy(a => Guid.NewGuid()).ToList();
        for (int i = 0; i < spawnerCount; i++)
        {
            if (randomizedTiles.Count <= 0)
            {
                break;
            }

            Tile spawnTile = randomizedTiles[^1];
            randomizedTiles.RemoveAt(randomizedTiles.Count - 1);
            if (i >= aiSpawners.Count)
            {
                AISpawner spawner = Instantiate(aiSpawner, Vector3.zero, Quaternion.identity);
                spawner.UpdatePosition(spawnTile);
                spawner.UpdatePath(new Vector3(0f, 0f, 0f), 5f);
                aiSpawners.Add(spawner);
            }
            else
            {
                aiSpawners[i].UpdatePosition(spawnTile);
                aiSpawners[i].UpdatePath(new Vector3(0f, 0f, 0f), 5f);
            }
        }
    }

    private void UpdateAIs()
    {
        MotherTreeOrchestrator motherTree = GameManager.Instance.GetMotherTree();
        foreach (AI ai in aiList)
        {
            List<Vector3> aiPath = aiPaths[ai];
            Vector3 target = aiPath[0];
            Vector3 aiPosition = ai.transform.position;
            if (Mathf.Pow(aiPosition.x - target.x, 2) + Mathf.Pow(aiPosition.z - target.z, 2) <= TARGET_RADIUS && aiPath.Count > 1)
            {
                aiPath.RemoveAt(0);
                target = aiPath[0];
            }

            if (motherTree != null && ai.IsInAttackRange(motherTree.transform.position))
            {
                ai.Attack(motherTree.GameObject());
            }
            else
            {
                ai.MoveTo(target);
            }
        } 
    }
}
