using System.Collections.Generic;
using System.Linq;
using Script.Decorators;
using Script.Decorators.Buildings;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

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
    private Transform BuildingSpawn;

    public BuildingDecorator CurrentBuilding { get; private set; }

    private List<Tile> Neighbours = new List<Tile>();

    public List<TileDecorator> TileDecorators;

    public float AICost { get; set; } = float.MaxValue;
    public static Dictionary<TileBuildingType, GameObject> BuildingCache = null;
    
    public List<RootsOnTile> Roots;
    
    public void Awake()
    {
        GenerateBuildingCache();
        TileDecorators = GetComponentsInChildren<TileDecorator>().ToList();
        Roots = GetComponentsInChildren<RootsOnTile>().ToList();
        SetNeighboursTile(new List<Tile>());
        SetActiveBuildingTile(TileBuildingType.NONE);
        Roots.ForEach(b => b.gameObject.SetActive(false));
    }

    public void GenerateBuildingCache()
    {
        if (BuildingCache is not null) 
            return;
        
        BuildingCache = new Dictionary<TileBuildingType, GameObject>();
        
        //We load all prefabs in the Buildings folder.
        foreach (Object building in Resources.LoadAll("Buildings"))
        {
            BuildingDecorator decorator = building.GetComponent<BuildingDecorator>();
            BuildingCache.Add(decorator.BuildingType, building.GameObject());
        }
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
        if(CurrentBuilding is not null || type == TileBuildingType.NONE)
            Destroy(CurrentBuilding);

        if (type == TileBuildingType.NONE)
            return;
        
        CurrentBuilding = Instantiate(BuildingCache[type], BuildingSpawn).GetComponent<BuildingDecorator>();
        CurrentBuilding.transform.SetParent(transform);
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

    public TileBuildingType GetCurrentBuildingType()
    {
        return CurrentBuilding?.BuildingType ?? TileBuildingType.NONE;
    }
}
