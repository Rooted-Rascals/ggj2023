using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType
{
    Grass,
    Water,
    Rock,
    Sand
};

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Vector3Int position;

    [SerializeField]
    private GameObject Halo;
    [SerializeField]
    private GameObject FogOfWar;

    [SerializeField]
    private TileType TileType;


    public List<TileDecorator> Tiles = new List<TileDecorator>();

    public void SetActiveTile(TileType type, bool fogOfWar)
    {
        TileType = type;
        Tiles.ForEach(b => b.gameObject.SetActive(false)) ;
        if (fogOfWar)
            FogOfWar.SetActive(true);
        else
        {
            FogOfWar.SetActive(false);
            Tiles.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);
        }
            
    }

    public void SetPosition(Vector3Int newPosition)
    {
        position = newPosition;
    }


    public void SetHalo(bool value)
    {
        Halo.SetActive(value);
    }
}
