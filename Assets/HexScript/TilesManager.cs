using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField]
    private int gridsize = 25;


    [SerializeField]
    private GameObject GrassTile;


    // Start is called before the first frame update
    void Start()
    {

        for (int i = -gridsize; i <= gridsize; i++)
        {
            Vector3 SpawnPosition = HexCoordinates.ConvertOffsetToPosition(new Vector3Int(0, i, -i));
            GameObject t = Instantiate(GrassTile, SpawnPosition, Quaternion.identity);
            t.GetComponent<GrassTile>().SetPosition(new Vector3Int(0, i, -i));
            t.name = $"x:{0}_y:{i}_z:{-i}";
        }

        for (int i = 0; i > -gridsize; i--)
        {
            int x = -(gridsize + i);
            int y = gridsize;
            int z = i;
            while(z <= gridsize)
            {
                Vector3 SpawnPosition = HexCoordinates.ConvertOffsetToPosition(new Vector3Int(x, y, z));
                GameObject t = Instantiate(GrassTile, SpawnPosition, Quaternion.identity);
                t.GetComponent<GrassTile>().SetPosition(new Vector3Int(x, y, z));
                t.name = $"x:{x}_y:{y}_z:{z}";

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
                Vector3 SpawnPosition = HexCoordinates.ConvertOffsetToPosition(new Vector3Int(x, y, z));
                GameObject t = Instantiate(GrassTile, SpawnPosition, Quaternion.identity);
                t.GetComponent<GrassTile>().SetPosition(new Vector3Int(x, y, z));
                t.name = $"x:{x}_y:{y}_z:{z}";

                z++;
                y--;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
