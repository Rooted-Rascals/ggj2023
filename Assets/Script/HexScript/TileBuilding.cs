using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBuildingType
{
    MotherTree,
    Roots
};

public class TileBuilding : MonoBehaviour
{
    [SerializeField]
    public TileBuildingType type;
}
