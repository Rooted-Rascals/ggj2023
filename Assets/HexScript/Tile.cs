using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Vector3Int position;

    public enum TileType
    {
        Grass,
        Water,
        Rock
    };

    [SerializeField]
    private GameObject WaterTile;
    [SerializeField]
    private GameObject GrassTile;
    [SerializeField]
    private GameObject RockTile;
    [SerializeField]
    private GameObject SandTile;

    [SerializeField]
    private TileType tileType = TileType.Grass;


    public void SetPosition(Vector3Int newPosition)
    {
        position = newPosition;
    }

    public void SetTileType(int type)
    {
        if (type == 0)
        {
            tileType = TileType.Grass;
            GrassTile.SetActive(true);
            WaterTile.SetActive(false);
            RockTile.SetActive(false);
            SandTile.SetActive(false);
        }
        else if (type == 1)
        {
            tileType = TileType.Water;
            GrassTile.SetActive(false);
            WaterTile.SetActive(true);
            RockTile.SetActive(false);
            SandTile.SetActive(false);
        }
        else if (type == 2)
        {
            tileType = TileType.Rock;
            GrassTile.SetActive(false);
            WaterTile.SetActive(false);
            RockTile.SetActive(true);
            SandTile.SetActive(false);
        }
        else if (type == 3)
        {
            tileType = TileType.Rock;
            GrassTile.SetActive(false);
            WaterTile.SetActive(false);
            RockTile.SetActive(false);
            SandTile.SetActive(true);
        }
    }

}
