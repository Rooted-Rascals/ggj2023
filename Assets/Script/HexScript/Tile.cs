using System.Collections.Generic;
using System.Linq;
using Script.Decorators;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
    private GameObject FogOfWar;

    [SerializeField]
    private TileType CurrentTyleType;

    [SerializeField]
    private List<TileBuildingType> CurrentBuildingType;

    private List<Tile> Neighbours = new List<Tile>();

    public List<TileBuilding> Buildings;
    public List<RootsOnTile> Roots;
    public List<TileDecorator> TileDecorators;

    public float AICost { get; set; } = float.MaxValue;
    
    public void Awake()
    {
        TileDecorators = GetComponentsInChildren<TileDecorator>().ToList();
        Buildings = GetComponentsInChildren<TileBuilding>().ToList();
        Roots = GetComponentsInChildren<RootsOnTile>().ToList();
        SetNeighboursTile(new List<Tile>());
        SetActiveBuildingTile(TileBuildingType.NONE);
        Roots.ForEach(b => b.gameObject.SetActive(false));
    }

    public void SetNeighboursTile(List<Tile> neighbours)
    {
        Neighbours = neighbours;
    }

    public List<Tile> GetNeighboursTile()
    {
        return Neighbours;
    }

    public List<TileBuildingType> GetBuildingsTile()
    {
        return CurrentBuildingType;
    }

    public void SetActiveBuildingTile(TileBuildingType type)
    {
        CurrentBuildingType.Add(type);
        Buildings.ForEach(b => b.gameObject.SetActive(false));
        if(type != TileBuildingType.NONE)
            Buildings.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);

        SetNeighboursActive();
    }

    public void SetRootsTile(RootsType type)
    {
        Roots.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);
        gameObject.GetComponentInChildren<TileDecorator>().RootsList.Add(type);
        gameObject.GetComponentInChildren<TileDecorator>().hasRoots = true;

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
}
