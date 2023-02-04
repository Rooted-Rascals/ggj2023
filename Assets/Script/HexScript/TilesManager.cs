using Script.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField]
    private int gridsize = 25;


    [SerializeField]
    private GameObject Tile;

    [SerializeField]
    private GameObject TilesParentFolder;

    private Dictionary<Vector3Int, GameObject> TilesMaps = new Dictionary<Vector3Int, GameObject>();
    

    private Vector3Int[] NeighbourCoords = new Vector3Int[]
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



        foreach(KeyValuePair <Vector3Int, GameObject> Tile  in TilesMaps)
        {
            List<GameObject> neighbours = GetNeighbours(Tile.Value, Tile.Key);
            Tile.Value.GetComponent<Tile>().SetNeighboursTile(neighbours);
        }

        GameObject spawn = TilesMaps[new Vector3Int(0, 0, 0)];

        spawn.GetComponent<Tile>().SetActiveTile(TileType.Grass, false);
        spawn.GetComponent<Tile>().SetActiveBuildingTile(TileBuildingType.MotherTree);
    }

    private void InstantiateTile(Vector3Int position)
    {
        Vector3 SpawnPosition = HexCoordinates.ConvertOffsetToPosition(position);
        GameObject t = Instantiate(Tile, SpawnPosition, Quaternion.identity);
        t.GetComponent<Tile>().SetPosition(position);
        t.name = $"x:{position.x}_y:{position.y}_z:{position.z}";
        t.transform.parent = TilesParentFolder.transform;
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
        TilesMaps.Add(position, t);
    }

    private List<GameObject> GetNeighbours(GameObject obj, Vector3Int position)
    {
        List<GameObject> neighbours = new List<GameObject>();

        foreach(Vector3Int neighbourCoord in NeighbourCoords)
        {
            if(TilesMaps.TryGetValue(position + neighbourCoord, out GameObject neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    void Update()
    {
        
    }
}
