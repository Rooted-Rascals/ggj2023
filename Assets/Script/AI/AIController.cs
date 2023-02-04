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

    private float delay = 0f;
    void Start()
    {
        UpdateSpawnPoints();
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
        List<Tile> spawnPossibleTiles = AIPathManager.Instance.GetSpawnPossibleTiles();
        List<Tile> randomizedTiles = spawnPossibleTiles.OrderBy(a => Guid.NewGuid()).ToList();
        while (aiSpawners.Count < spawnerCount)
        {
            if (randomizedTiles.Count <= 0)
            {
                break;
            }

            Tile spawnTile = randomizedTiles[^1];
            randomizedTiles.RemoveAt(randomizedTiles.Count - 1);
            aiSpawners.Add(Instantiate(aiSpawner, spawnTile.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity));
        }
    }

    private void UpdateAIs()
    {
        MotherTree motherTree = GameManager.Instance.GetMotherTree();
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

            if (ai.IsInAttackRange(motherTree.transform.position))
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
