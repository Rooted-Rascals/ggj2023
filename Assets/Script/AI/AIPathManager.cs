using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Decorators;
using TMPro;
using UnityEngine;

public class AIPathManager : MonoBehaviour
{
    [SerializeField]
    private bool debugEnable = false;
    private static AIPathManager INSTANCE;

    public static AIPathManager Instance
    {
        get
        {
            return INSTANCE;
        }
    }
    
    [SerializeField] private TilesManager tilesManager;
    private List<Tile> spawnPossibleTiles = new List<Tile>();

    public List<Tile> GetSpawnPossibleTiles()
    {
        return spawnPossibleTiles;
    }

    private const uint BASE_COST = 1;
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
        DontDestroyOnLoad(this);
    }

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
                toVisitTiles.Enqueue(neighbour);
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
                TileDecorator tileDecorator = neighbour.GetComponentInChildren<TileDecorator>();
                if (tileDecorator == null)
                {
                    if (!TileIsBlocker(neighbour))
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
        TileType tileType = tile.GetTileType();

        return tileType is TileType.Rock or TileType.Water;
    }

    void Update()
    {
        // Debug Helping Code
        if (debugEnable)
        {
            foreach (Tile tile in tilesManager.GetTiles())
            {
                TextMeshProUGUI ui = tile.GetComponentInChildren<TextMeshProUGUI>();
                if (ui)
                {
                    ui.text = $"{tile.AICost}";
                }
            }
        }
    }

    void Start()
    {
        UpdateAIGrid();
    }
}
