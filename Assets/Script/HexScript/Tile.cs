using System;
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

    [SerializeField]
    private TileBuildingType TileBuildingType;

    private List<GameObject> Neighbours;

    [SerializeField]
    private List<TileDecorator> Tiles = new List<TileDecorator>();

    [SerializeField]
    private List<TileBuilding> Buildings = new List<TileBuilding>();

    public void SetNeighboursTile(List<GameObject> neighbours)
    {
        Neighbours = neighbours;
    }

    public void SetActiveBuildingTile(TileBuildingType type)
    {
        Buildings.ForEach(b => b.gameObject.SetActive(false));
        Buildings.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);
        SetNeighboursActive();
    }

    public void SetNeighboursActive()
    {
        foreach (GameObject item in Neighbours)
        {
            TileType type = item.GetComponent<Tile>().GetTileType();
            item.GetComponent<Tile>().SetActiveTile(type, false);
            
        }
    }

    public TileType GetTileType()
    {
        return TileType;
    }

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
