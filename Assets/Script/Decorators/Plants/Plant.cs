using UnityEngine;

namespace Script.Decorators.Plants
{
    public enum PlantType
    {
        NONE,
        MOTHERTREE,
        CACTUS,
        LILYPAD,
        MUSHROOM
    };
    
    public abstract class Plant : MonoBehaviour
    {
        public abstract PlantType PlantType { get; }
    }
}