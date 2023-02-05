using System;
using System.Collections.Generic;
using Script.Decorators.Plants;
using UnityEngine;
using Random = UnityEngine.Random;

public class TilesManager : MonoBehaviour
{
    [SerializeField]
    private int gridsize = 120;

    [SerializeField]
    private Tile tile;

    [SerializeField]
    private GameObject tilesParentFolder;

    private Dictionary<Vector3Int, Tile> tilesMaps = new Dictionary<Vector3Int, Tile>();
    private List<Tile> tiles = new List<Tile>(); 

    private Vector3Int[] neighbourCoords = new Vector3Int[]
        {
            new Vector3Int(1, -1, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 1, -1),
            new Vector3Int(0, -1, 1),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 1)
        };

    public static float GridWidth { get; private set; }
    public static float GridHeight { get; private set; }
    
    void Awake()
    {

        for (int i = -gridsize; i <= gridsize; i++)
        {
            InstantiateTile(new Vector3Int(0, i, -i));
        }

        for (int i = 0; i > -gridsize; i--)
        {
            int x = -(gridsize + i);
            int y = gridsize;
            int z = i;
            while(z <= gridsize)
            {
                InstantiateTile(new Vector3Int(x, y, z));

                z++;
                y--;
            }
        }

        for (int i = 0; i < gridsize; i++)
        {
            int x = (gridsize - i);
            int y = i;
            int z = -gridsize;
            while (y >= -gridsize)
            {
                InstantiateTile(new Vector3Int(x, y, z));

                z++;
                y--;
            }
        }

        foreach(KeyValuePair <Vector3Int, Tile> tile  in tilesMaps)
        {
            List<Tile> neighbours = GetNeighbours(tile.Value, tile.Key);
            tile.Value.GetComponent<Tile>().SetNeighboursTile(neighbours);
        }
    }

    private void Start()
    {
        AIController.Instance.UpdateAIGrid();
    }

    private void InstantiateTile(Vector3Int position)
    {
        Vector3 spawnPosition = HexCoordinates.ConvertOffsetToPosition(position);
        Tile t = Instantiate(tile, spawnPosition, Quaternion.identity);
        t.GetComponent<Tile>().SetPosition(position);
        t.name = $"x:{position.x}_y:{position.y}_z:{position.z}";
        t.transform.parent = tilesParentFolder.transform;
        if(position.x == gridsize || position.x == -gridsize ||
           position.y == gridsize || position.y == -gridsize ||
           position.z == gridsize || position.z == -gridsize)
            t.GetComponent<Tile>().SetActiveTile(BiomeType.Rock, true);
        else
        {
            int xcount = Random.Range(1, 11);
            if (xcount == 3)
                t.GetComponent<Tile>().SetActiveTile(BiomeType.Water, true);
            else if (xcount == 4)
                t.GetComponent<Tile>().SetActiveTile(BiomeType.Rock, true);
            else if (xcount <= 2)
                t.GetComponent<Tile>().SetActiveTile(BiomeType.Sand, true);
            else
                t.GetComponent<Tile>().SetActiveTile(BiomeType.Grass, true);
        }
        tilesMaps.Add(position, t);
        tiles.Add(t);

        if (t.transform.position.x > GridWidth)
            GridWidth = t.transform.position.x;
        
        if (t.transform.position.z > GridHeight)
            GridHeight = t.transform.position.z;
    }

    private List<Tile> GetNeighbours(Tile obj, Vector3Int position)
    {
        List<Tile> neighbours = new List<Tile>();

        foreach(Vector3Int neighbourCoord in neighbourCoords)
        {
            if(tilesMaps.TryGetValue(position + neighbourCoord, out Tile neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public Tile GetTile(Vector3Int index)
    {
        Tile tile = null;
        tilesMaps.TryGetValue(index, out tile);
        return tile;
    }

    public List<Tile> GetTiles()
    {
        return tiles;
    }

    public int GetGridSize()
    {
        return gridsize;
    }
}
