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


    // Start is called before the first frame update
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
            t.GetComponent<Tile>().SetTileType(2);
        else
        {
            int xcount = Random.Range(1, 11);
            if (xcount == 3)
                t.GetComponent<Tile>().SetTileType(1);
            else if (xcount == 4)
                t.GetComponent<Tile>().SetTileType(2);
            else if (xcount <= 2)
                t.GetComponent<Tile>().SetTileType(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
