using Script.Decorators;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootsSystem : MonoBehaviour
{
    private Tile Tile;

    private Dictionary<Vector3Int, string> NeighboursDirection = new Dictionary<Vector3Int, string>();

    private void Start()
    {
        Tile = GetComponent<Tile>();

        NeighboursDirection.Add(new Vector3Int(1, -1, 0), "z");
        NeighboursDirection.Add(new Vector3Int(-1, 1, 0), "z_minus");
        NeighboursDirection.Add(new Vector3Int(-1, 0, 1), "y");
        NeighboursDirection.Add(new Vector3Int(1, 0, -1), "y_minus");
        NeighboursDirection.Add(new Vector3Int(0, 1, -1), "x");
        NeighboursDirection.Add(new Vector3Int(0, -1, 1), "x_minus");
        
        
    }

    public void BuildRoots()
    {
        GetNeighbourBranchDirection();
    }

    public void GetNeighbourBranchDirection()
    {
        List<Tile> neighbours = Tile.GetNeighboursTile();
        Vector3Int position = Tile.GetPosition();

        foreach (Tile neighbour in neighbours)
        {
            List<TileBuildingType> ActiveBuildingType = neighbour.GetBuildingsTile();

            if (ActiveBuildingType.Contains(TileBuildingType.MotherTree))
            {
                Vector3Int neighbourPosition = neighbour.GetPosition();

                if (NeighboursDirection.TryGetValue(position - neighbourPosition, out string direction))
                {
                    switch (direction)
                    {
                        case "x":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsx);
                            break;
                        case "x_minus":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsx_minus);
                            break;
                        case "y":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsy);
                            break;
                        case "y_minus":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsy_minus);
                            break;
                        case "z":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsz);
                            break;
                        case "z_minus":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsz_minus);
                            break;
                    }
                    
                }
                else if (neighbour.GetComponentInChildren<TileDecorator>().hasRoots)
                {
                    switch (direction)
                    {
                        case "x":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsx);
                            neighbour.SetActiveBuildingTile(TileBuildingType.Rootsx_minus);
                            break;
                        case "x_minus":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsx_minus);
                            neighbour.SetActiveBuildingTile(TileBuildingType.Rootsx);
                            break;
                        case "y":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsy);
                            neighbour.SetActiveBuildingTile(TileBuildingType.Rootsy_minus);
                            break;
                        case "y_minus":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsy_minus);
                            neighbour.SetActiveBuildingTile(TileBuildingType.Rootsy);
                            break;
                        case "z":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsz);
                            neighbour.SetActiveBuildingTile(TileBuildingType.Rootsz_minus);
                            break;
                        case "z_minus":
                            Tile.SetActiveBuildingTile(TileBuildingType.Rootsz_minus);
                            neighbour.SetActiveBuildingTile(TileBuildingType.Rootsz);
                            break;
                    }
                }
            }
        }
    }
}
