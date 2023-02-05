#nullable enable
using System.Collections.Generic;
using System.Linq;
using Script.Decorators.Biomes;
using Script.Decorators.Plants;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public enum BiomeType
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
    public Biome CurrentBiome { get; private set; }

    [SerializeField]
    private Transform BuildingSpawn;

    public Plant CurrentBuilding { get; private set; }

    private List<Tile> Neighbours = new List<Tile>();

    public List<Biome> TileDecorators;

    public float AICost { get; set; } = float.MaxValue;
    public static Dictionary<PlantType, GameObject> BuildingCache = null;
    
    public List<RootsOnTile> Roots;
    
    public void Awake()
    {
        GenerateBuildingCache();
        TileDecorators = GetComponentsInChildren<Biome>().ToList();
        Roots = GetComponentsInChildren<RootsOnTile>().ToList();
        SetNeighboursTile(new List<Tile>());
        SetActiveBuildingTile(PlantType.NONE);
        Roots.ForEach(b => b.gameObject.SetActive(false));
    }

    public void GenerateBuildingCache()
    {
        if (BuildingCache is not null) 
            return;
        
        BuildingCache = new Dictionary<PlantType, GameObject>();
        
        //We load all prefabs in the Buildings folder.
        foreach (Object building in Resources.LoadAll("Buildings"))
        {
            Plant decorator = building.GetComponent<Plant>();
            BuildingCache.Add(decorator.PlantType, building.GameObject());
            Debug.Log($"Found {decorator.PlantType.ToString()}");
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

    public GameObject SetActiveBuildingTile(PlantType type)
    {
        if(CurrentBuilding is not null || type == PlantType.NONE)
            Destroy(CurrentBuilding);

        if (type == PlantType.NONE)
            return null;

        GameObject createdObject = Instantiate(BuildingCache[type], BuildingSpawn);
        CurrentBuilding = createdObject.GetComponent<Plant>();
        CurrentBuilding.transform.SetParent(transform);
        SetNeighboursActive(3);
        return createdObject;
    }

    public void SetRootsTile(RootsType? type)
    {
        if(type is null)
        {
            Roots.ForEach(b => b.gameObject.SetActive(false));
            gameObject.GetComponentInChildren<Biome>().hasRoots = false;
            CurrentBuilding = null;

        }
        else
        {
            Roots.FirstOrDefault(b => b.type == type).gameObject.SetActive(true);
            gameObject.GetComponentInChildren<Biome>().hasRoots = true;

            SetNeighboursActive(1);
        }
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
                    BiomeType type = tile.GetTileBiome();
                    tile.SetActiveTile(type, false);
                }
                if (size > 1)
                    alreadySeen = tile.SetNeighboursActive(size - 1, alreadySeen);
            }
        }

        return alreadySeen;
    }

    public BiomeType GetTileBiome()
    {
        return CurrentBiome.Type;
    }
    
    public void SetActiveTile(BiomeType type, bool fogOfWar)
    {
        CurrentBiome = TileDecorators.FirstOrDefault(b => b.Type == type);
        CurrentBiome.IsVisible = !fogOfWar;
        
        TileDecorators.ForEach(b => b.gameObject.SetActive(false)) ;
        if (fogOfWar)
            FogOfWar.SetActive(true);
        else
        {
            FogOfWar.SetActive(false);
            CurrentBiome.gameObject.SetActive(true);
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

    public PlantType GetCurrentBuildingType()
    {
        return CurrentBuilding?.PlantType ?? PlantType.NONE;
    }
}
