using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using Script.Decorators.Plants;
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
        foreach (AI ai in aiList)
        {
            List<Vector3> aiPath = aiPaths[ai];
            Vector3 target = aiPath[0];
            Vector3 aiPosition = ai.transform.position;

            Plant closestHit = null;
            float closestDistance = float.MaxValue;
            List<Tile> roots = GameManager.Instance.GetMotherTree().GetAllRoots();
            Vector3 updatedAiPosition = new Vector3(ai.transform.position.x, 0f, ai.transform.position.z);
            foreach (Tile rootTile in roots)
            {
                
                Plant building = rootTile.CurrentBuilding;
                if (building != null && ai.IsInAttackRange(building.transform.position))
                {
                    Vector3 updatedBuildingPosition = new Vector3(building.transform.position.x, 0f, building.transform.position.z);
                    Vector3 distance = (updatedAiPosition - updatedBuildingPosition);
                    if (distance.magnitude < closestDistance)
                    {
                        closestHit = building;
                        closestDistance = distance.magnitude;
                    }
                }
            }
            if (closestHit != null && closestHit.GameObject() != null)
            {
                ai.Attack(closestHit.GameObject());
            }
            else
            {
                if (Mathf.Pow(aiPosition.x - target.x, 2) + Mathf.Pow(aiPosition.z - target.z, 2) <= TARGET_RADIUS)
                {
                    if (aiPath.Count > 1)
                    {
                        aiPath.RemoveAt(0);
                        target = aiPath[0];
                    }
                    else
                    {
                        continue;
                    }
                }
                ai.MoveTo(target);
            }
        } 
    }
    
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            foreach (AI ai in aiList)
            {
                Gizmos.DrawWireSphere(ai.transform.position, ai.GetRange());
            }
        }
#endif
}
