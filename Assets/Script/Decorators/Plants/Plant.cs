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
        [SerializeField] private float waterConsumption = 1f;
        [SerializeField] private float waterGeneration = 0f;
        [SerializeField] private float energyGeneration = 0f;
        public abstract PlantType PlantType { get; }

        public virtual void UpdatePlant()
        {
            return; // No-Op plant
        }

        public virtual float GetWaterConsumption()
        {
            return waterConsumption;
        }

        public virtual float GetWaterGeneration()
        {
            return waterGeneration;
        }

        public virtual float GetEnergyGeneration()
        {
            return energyGeneration;
        }
    }
}