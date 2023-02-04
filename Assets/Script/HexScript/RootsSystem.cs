using Script.Manager;
using System.Collections.Generic;
using Script.Decorators.Biomes;
using Script.Decorators.Plants;
using UnityEngine;

public enum RootsType
{
    Rootsx,
    Rootsx_minus,
    Rootsy,
    Rootsy_minus,
    Rootsz,
    Rootsz_minus,
};

public class RootsSystem : MonoBehaviour
{
    private Dictionary<Vector3Int, string> NeighboursDirection = new Dictionary<Vector3Int, string>();

    private void Start()
    {
        NeighboursDirection.Add(new Vector3Int(1, -1, 0), "y_minus");
        NeighboursDirection.Add(new Vector3Int(-1, 1, 0), "y");
        NeighboursDirection.Add(new Vector3Int(-1, 0, 1), "z_minus");
        NeighboursDirection.Add(new Vector3Int(1, 0, -1), "z");
        NeighboursDirection.Add(new Vector3Int(0, 1, -1), "x");
        NeighboursDirection.Add(new Vector3Int(0, -1, 1), "x_minus");
        
        
    }

    public void BuildRoots()
    {
        Tile selectedTile = MouseManager.Instance.CurrentSelectedObject.GetComponent<Tile>();
        GetNeighbourBranchDirection(selectedTile); 
        GameManager.Instance.GetMotherTree().AddRoots(selectedTile);
        AIController.Instance.UpdateAIGrid();
    }

    public void GetNeighbourBranchDirection(Tile tile)
    {
        List<Tile> neighbours = tile.GetNeighboursTile();
        Vector3Int position = tile.GetPosition();

        foreach (Tile neighbour in neighbours)
        {
            PlantType ActiveBuildingType = neighbour.GetCurrentBuildingType();
            Vector3Int neighbourPosition = neighbour.GetPosition();

            Biome biome = neighbour.gameObject.GetComponentInChildren<Biome>();
            //si conecter a labre
            if (biome && biome.hasRoots || ActiveBuildingType == PlantType.MOTHERTREE)
            {
                if (NeighboursDirection.TryGetValue(position - neighbourPosition, out string direction))
                {
                    switch (direction)
                    {
                        case "x":
                            tile.SetRootsTile(RootsType.Rootsx);
                            neighbour.SetRootsTile(RootsType.Rootsx_minus);
                            break;
                        case "x_minus":
                            tile.SetRootsTile(RootsType.Rootsx_minus);
                            neighbour.SetRootsTile(RootsType.Rootsx);
                            break;
                        case "y":
                            tile.SetRootsTile(RootsType.Rootsy);
                            neighbour.SetRootsTile(RootsType.Rootsy_minus);
                            break;
                        case "y_minus":
                            tile.SetRootsTile(RootsType.Rootsy_minus);
                            neighbour.SetRootsTile(RootsType.Rootsy);
                            break;
                        case "z":
                            tile.SetRootsTile(RootsType.Rootsz);
                            neighbour.SetRootsTile(RootsType.Rootsz_minus);
                            break;
                        case "z_minus":
                            tile.SetRootsTile(RootsType.Rootsz_minus);
                            neighbour.SetRootsTile(RootsType.Rootsz);
                            break;
                    }
                    break;
                }
           }
            
        }
    }

    public static bool CheckNeighboursForRoots(Tile tile)
    {
        bool isNearRoots = false;
        List<Tile> neighbours = tile.GetNeighboursTile();
        Vector3Int position = tile.GetPosition();

        foreach (Tile neighbour in neighbours)
        {
            PlantType ActiveBuildingType = neighbour.GetCurrentBuildingType();
            Vector3Int neighbourPosition = neighbour.GetPosition();

            Biome biome = neighbour.gameObject.GetComponentInChildren<Biome>();
            if (biome && biome.hasRoots || ActiveBuildingType == PlantType.MOTHERTREE)
            {
                isNearRoots = true;
                break;
            }

        }

        return isNearRoots;
    }
}
