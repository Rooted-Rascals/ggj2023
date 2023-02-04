#nullable enable
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
    public TileDecorator CurrentTyleDecorator { get; private set; }

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
            Debug.Log($"Found {decorator.BuildingType.ToString()}");
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
        SetNeighboursActive(3);
    }

    public void SetRootsTile(RootsType type)
    {
        Roots.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);
        gameObject.GetComponentInChildren<TileDecorator>().RootsList.Add(type);
        gameObject.GetComponentInChildren<TileDecorator>().hasRoots = true;

        SetNeighboursActive(1);
    }

    public List<Vector3Int> SetNeighboursActive(int size, List<Vector3Int>? alreadySeen = null)
    {
        if(alreadySeen == null)
            alreadySeen = new();

        foreach (Tile item in Neighbours)
        {
            Tile tile = item.GetComponent<Tile>();
            if (alreadySeen != null)
            {
                Vector3Int position = tile.GetPosition();

                if (!alreadySeen.Contains(position))
                {
                    TileType type = tile.GetTileType();
                    tile.SetActiveTile(type, false);
                }
                if (size > 1)
                    alreadySeen = tile.SetNeighboursActive(size - 1, alreadySeen);
            }
        }

        return alreadySeen;
    }

    public TileType GetTileType()
    {
        return CurrentTyleDecorator.Type;
    }
    
    public void SetActiveTile(TileType type, bool fogOfWar)
    {
        CurrentTyleDecorator = TileDecorators.FirstOrDefault(b => b.Type == type);
        CurrentTyleDecorator.IsVisible = !fogOfWar;
        
        TileDecorators.ForEach(b => b.gameObject.SetActive(false)) ;
        if (fogOfWar)
            FogOfWar.SetActive(true);
        else
        {
            FogOfWar.SetActive(false);
            CurrentTyleDecorator.gameObject.SetActive(true);
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
