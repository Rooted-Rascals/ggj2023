using Script.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TilesManager : MonoBehaviour
{
    [SerializeField]
    private int gridsize = 25;


    [SerializeField]
    private Tile tile;

    [SerializeField]
    private GameObject tilesParentFolder;

    private Dictionary<Vector3Int, Tile> tilesMaps = new Dictionary<Vector3Int, Tile>();
    

    private Vector3Int[] neighbourCoords = new Vector3Int[]
        {
            new Vector3Int(1, -1, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 1, -1),
            new Vector3Int(0, -1, 1),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 1)
        };


    // Start is called before the first frame update
    void Start()
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

        Tile spawn = tilesMaps[new Vector3Int(0, 0, 0)];

        spawn.GetComponent<Tile>().SetActiveTile(TileType.Grass, false);
        spawn.GetComponent<Tile>().SetActiveBuildingTile(TileBuildingType.MotherTree);
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
            t.GetComponent<Tile>().SetActiveTile(TileType.Rock, true);
        else
        {
            int xcount = Random.Range(1, 11);
            if (xcount == 3)
                t.GetComponent<Tile>().SetActiveTile(TileType.Water, true);
            else if (xcount == 4)
                t.GetComponent<Tile>().SetActiveTile(TileType.Rock, true);
            else if (xcount <= 2)
                t.GetComponent<Tile>().SetActiveTile(TileType.Sand, true);
            else
                t.GetComponent<Tile>().SetActiveTile(TileType.Grass, true);
        }
        tilesMaps.Add(position, t);
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
}
