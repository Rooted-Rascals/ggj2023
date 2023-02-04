using Script.Decorators;
using Script.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private Tile Tile;

    private Dictionary<Vector3Int, string> NeighboursDirection = new Dictionary<Vector3Int, string>();

    private void Start()
    {
        Tile = GetComponent<Tile>();

        NeighboursDirection.Add(new Vector3Int(1, -1, 0), "y_minus");
        NeighboursDirection.Add(new Vector3Int(-1, 1, 0), "y");
        NeighboursDirection.Add(new Vector3Int(-1, 0, 1), "z_minus");
        NeighboursDirection.Add(new Vector3Int(1, 0, -1), "z");
        NeighboursDirection.Add(new Vector3Int(0, 1, -1), "x");
        NeighboursDirection.Add(new Vector3Int(0, -1, 1), "x_minus");
        
        
    }

    public void BuildRoots()
    {
        Tile = MouseManager.Instance.CurrentSelectedObject.GetComponent<Tile>();
        print("build");
        GetNeighbourBranchDirection();
    }

    public void GetNeighbourBranchDirection()
    {
        List<Tile> neighbours = Tile.GetNeighboursTile();
        Vector3Int position = Tile.GetPosition();

        foreach (Tile neighbour in neighbours)
        {
            List<TileBuildingType> ActiveBuildingType = neighbour.GetBuildingsTile();
            Vector3Int neighbourPosition = neighbour.GetPosition();

            if (ActiveBuildingType.Contains(TileBuildingType.MotherTree))
            {
                
                print(neighbour.GetComponentInChildren<TileDecorator>().hasRoots);
                print(neighbour.GetComponentInChildren<TileDecorator>().RootsList.Count);

                if (NeighboursDirection.TryGetValue(position - neighbourPosition, out string direction))
                {
                    switch (direction)
                    {
                        case "x":
                            Tile.SetRootsTile(RootsType.Rootsx);
                            break;
                        case "x_minus":
                            Tile.SetRootsTile(RootsType.Rootsx_minus);
                            break;
                        case "y":
                            Tile.SetRootsTile(RootsType.Rootsy);
                            break;
                        case "y_minus":
                            Tile.SetRootsTile(RootsType.Rootsy_minus);
                            break;
                        case "z":
                            Tile.SetRootsTile(RootsType.Rootsz);
                            break;
                        case "z_minus":
                            Tile.SetRootsTile(RootsType.Rootsz_minus);
                            break;
                    }
                }
                    
            }

            TileDecorator tileDecorator = neighbour.gameObject.GetComponentInChildren<TileDecorator>();
            if (tileDecorator && tileDecorator.hasRoots)
            {
                if (NeighboursDirection.TryGetValue(position - neighbourPosition, out string direction))
                {
                    print("neighbour");
                    switch (direction)
                    {
                        case "x":
                            Tile.SetRootsTile(RootsType.Rootsx);
                            neighbour.SetRootsTile(RootsType.Rootsx_minus);
                            break;
                        case "x_minus":
                            Tile.SetRootsTile(RootsType.Rootsx_minus);
                            neighbour.SetRootsTile(RootsType.Rootsx);
                            break;
                        case "y":
                            Tile.SetRootsTile(RootsType.Rootsy);
                            neighbour.SetRootsTile(RootsType.Rootsy_minus);
                            break;
                        case "y_minus":
                            Tile.SetRootsTile(RootsType.Rootsy_minus);
                            neighbour.SetRootsTile(RootsType.Rootsy);
                            break;
                        case "z":
                            Tile.SetRootsTile(RootsType.Rootsz);
                            neighbour.SetRootsTile(RootsType.Rootsz_minus);
                            break;
                        case "z_minus":
                            Tile.SetRootsTile(RootsType.Rootsz_minus);
                            neighbour.SetRootsTile(RootsType.Rootsz);
                            break;
                    }
                }
           }
            
        }
    }
}
