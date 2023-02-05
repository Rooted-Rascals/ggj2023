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
    private Dictionary<AI, Vector3> aiTargets = new Dictionary<AI, Vector3>();

    [SerializeField]
    private float spawnDelay = 5f;
    [SerializeField]
    private float spawnerCount = 2f;

    [SerializeField] private AIPathManager aiPathManager;

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
        delay += Time.deltaTime;

        UpdateAIs();
        SpawnAIs();
    }

    private void OnDrawGizmos()
    {
        foreach (AI ai in aiList)
        {
            Vector3 aiTarget = aiTargets[ai];
            Gizmos.color = Color.red;
            Vector3 updatedTarget = new Vector3(aiTarget.x, ai.transform.position.y, aiTarget.z);
            Gizmos.DrawLine(ai.transform.position, updatedTarget);
        }
    }

    private void SpawnAIs()
    {
        if (delay >= spawnDelay)
        {
            delay = 0;
            if (aiSpawners.Count <= 0)
            {
                return;
            }
            AI spawnedAi = aiSpawners[Random.Range(0, aiSpawners.Count)].SpawnAI();
            aiList.Add(spawnedAi);
            aiTargets.Add(spawnedAi, spawnedAi.transform.position);
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
            Vector3 spawnerPosition = spawnTile.transform.position + new Vector3(0f, 2f, 0f);
            if (i >= aiSpawners.Count)
            {
                aiSpawners.Add(Instantiate(aiSpawner, spawnerPosition, Quaternion.identity));
            }
            else
            {
                aiSpawners[i].transform.position = spawnerPosition;
            }
        }
    }

    private void UpdateAIs()
    {
        MotherTreeOrchestrator motherTree = GameManager.Instance.GetMotherTree();
        foreach (AI ai in aiList)
        {
            Vector3 target = aiTargets[ai];
            Vector3 aiPosition = ai.transform.position;
            if (Mathf.Pow(aiPosition.x - target.x, 2) + Mathf.Pow(aiPosition.z - target.z, 2) <= TARGET_RADIUS)
            {
                Tile tile = ai.GetTileUnderneath();
                if (tile != null)
                {
                    float lowestCost = tile.AICost;
                    foreach (Tile neighbour in tile.GetNeighboursTile())
                    {
                        if (lowestCost > neighbour.AICost)
                        {
                            lowestCost = neighbour.AICost;
                            target = HexCoordinates.ConvertOffsetToPosition(neighbour.GetPosition());
                        }
                    }
                    aiTargets[ai] = target;
                }
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
