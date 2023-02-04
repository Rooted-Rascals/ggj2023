using UnityEngine;

namespace Script.Decorators.Buildings
{
    public enum TileBuildingType
    {
        NONE,
        MOTHERTREE,
        CACTUS,
    };
    
    public abstract class BuildingDecorator : MonoBehaviour
    {
        public abstract TileBuildingType TypeBuilder { get; }
    }
}