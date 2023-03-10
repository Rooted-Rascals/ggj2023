using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Decorators;
using Script.Decorators.Biomes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AIPathManager : MonoBehaviour
{
    [SerializeField]
    private bool debugEnable = false;

    [SerializeField] private TilesManager tilesManager;
    private List<Tile> spawnPossibleTiles = new List<Tile>();

    public List<Tile> GetSpawnPossibleTiles()
    {
        return spawnPossibleTiles;
    }

    private const uint BASE_COST = 1;


    public void UpdateAIGrid()
    {
        UpdateGridAICost();
        UpdateSpawningGridTiles();
    }
    
    void UpdateGridAICost()
    {
        foreach (Tile tile in tilesManager.GetTiles())
        {
            tile.AICost = float.MaxValue;
        }
        
        HashSet<Tile> visitedTile = new HashSet<Tile>();
        Queue<Tile> toVisitTiles = new Queue<Tile>();
        Tile spawnTile = tilesManager.GetTile(new Vector3Int(0, 0, 0));
        spawnTile.AICost = 0;
        toVisitTiles.Enqueue(spawnTile);
        while (toVisitTiles.Count > 0)
        {
            Tile visiting = toVisitTiles.Dequeue();
            if (visitedTile.Contains(visiting))
            {
                continue;
            }

            float lowestNeighbourCost = float.MaxValue;
            foreach (Tile neighbour in visiting.GetNeighboursTile())
            {
                lowestNeighbourCost = Mathf.Min(neighbour.AICost, lowestNeighbourCost);
                if (!TileIsBlocker(visiting))
                {
                    toVisitTiles.Enqueue(neighbour);
                }
            }

            if (visiting != spawnTile)
            {
                if (TileIsBlocker(visiting))
                {
                    visiting.AICost = float.MaxValue;
                }
                else
                {
                    visiting.AICost = lowestNeighbourCost + BASE_COST;
                }
            }

            visitedTile.Add(visiting);
        }

        // spawnTile.AICost = float.MaxValue;
    }

    void UpdateSpawningGridTiles()
    {
        HashSet<Tile> visitedTile = new HashSet<Tile>();
        HashSet<Tile> spawningTiles = new HashSet<Tile>();
        Queue<Tile> toVisitTiles = new Queue<Tile>();
        Tile spawnTile = tilesManager.GetTile(new Vector3Int(0, 0, 0));
        toVisitTiles.Enqueue(spawnTile);
        while (toVisitTiles.Count > 0)
        {
            Tile visiting = toVisitTiles.Dequeue();
            if (visitedTile.Contains(visiting))
            {
                continue;
            }

            foreach (Tile neighbour in visiting.GetNeighboursTile())
            {
                Biome biome = neighbour.GetComponentInChildren<Biome>();
                if (biome == null)
                {
                    if (!TileIsBlocker(neighbour) && neighbour.AICost < float.MaxValue)
                    {
                        spawningTiles.Add(neighbour);
                    }
                }
                else
                {
                    toVisitTiles.Enqueue(neighbour);
                }
            }

            visitedTile.Add(visiting);
        }

        spawnPossibleTiles = spawningTiles.ToList();
    }

    private bool TileIsBlocker(Tile tile)
    {
        BiomeType biomeType = tile.GetTileBiome();

        return biomeType is BiomeType.Rock or BiomeType.Water;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Debug Helping Code
        foreach (Tile tile in tilesManager.GetTiles())
        {
            string label = float.MaxValue == tile.AICost ? "INF" : $"{tile.AICost}";
            
            Handles.Label(tile.transform.position + Vector3.up * 1.5f, label);
        }
    }
#endif

    void Start()
    {
        UpdateAIGrid();
    }
}
