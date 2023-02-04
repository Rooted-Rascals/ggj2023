using System.Collections.Generic;
using System.Linq;
using Script.Decorators;
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
    private Vector3Int Position;

    [SerializeField]
    private GameObject Halo;
    [SerializeField]
    private GameObject FogOfWar;

    [SerializeField]
    private TileType CurrentTyleType;

    [SerializeField]
    private TileBuildingType CurrentBuildingType;

    private List<Tile> Neighbours = new List<Tile>();

    public List<TileBuilding> Buildings;
    public List<TileDecorator> TileDecorators;

    public float AICost { get; set; } = float.MaxValue;
    
    public void Awake()
    {
        TileDecorators = GetComponentsInChildren<TileDecorator>().ToList();
        Buildings = GetComponentsInChildren<TileBuilding>().ToList();
        SetNeighboursTile(new List<Tile>());
        SetActiveBuildingTile(TileBuildingType.NONE);
    }

    public void SetNeighboursTile(List<Tile> neighbours)
    {
        Neighbours = neighbours;
    }

    public List<Tile> GetNeighboursTile()
    {
        return Neighbours;
    }

    public void SetActiveBuildingTile(TileBuildingType type)
    {
        Buildings.ForEach(b => b.gameObject.SetActive(false));
        if(type != TileBuildingType.NONE)
            Buildings.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);
        
        SetNeighboursActive();
    }

    public void SetNeighboursActive()
    {
        foreach (Tile item in Neighbours)
        {
            TileType type = item.GetComponent<Tile>().GetTileType();
            item.GetComponent<Tile>().SetActiveTile(type, false);
            
        }
    }

    public TileType GetTileType()
    {
        return CurrentTyleType;
    }
    
    public void SetActiveTile(TileType type, bool fogOfWar)
    {
        CurrentTyleType = type;
        TileDecorators.ForEach(b => b.gameObject.SetActive(false)) ;
        if (fogOfWar)
            FogOfWar.SetActive(true);
        else
        {
            FogOfWar.SetActive(false);
            TileDecorators.FirstOrDefault(b => b.Type == type).gameObject.SetActive(true);
        }
            
    }

    public void SetPosition(Vector3Int newPosition)
    {
        Position = newPosition;
    }

    public Vector3Int GetPosition()
    {
        return Position;
    }


    public void SetHalo(bool value)
    {
        Halo.SetActive(value);
    }
}
