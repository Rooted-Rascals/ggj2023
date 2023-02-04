using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBuildingType
{
    NONE,
    MotherTree,
    Roots
};

public class TileBuilding : MonoBehaviour
{
    [SerializeField]
    public TileBuildingType type;
}
