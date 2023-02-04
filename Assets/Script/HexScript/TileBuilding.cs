using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBuildingType
{
    NONE,
    MotherTree,
    Rootsx,
    Rootsx_minus,
    Rootsy,
    Rootsy_minus,
    Rootsz,
    Rootsz_minus,
};

public class TileBuilding : MonoBehaviour
{
    [SerializeField]
    public TileBuildingType type;
}
