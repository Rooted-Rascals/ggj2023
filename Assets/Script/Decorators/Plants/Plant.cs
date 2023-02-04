using UnityEngine;

namespace Script.Decorators.Plants
{
    public enum PlantType
    {
        NONE,
        MOTHERTREE,
        CACTUS,
        LILYPAD,
        MUSHROOM,
        LEAF
    };
    
    public abstract class Plant : MonoBehaviour
    {
        public abstract PlantType PlantType { get; }
    }
}