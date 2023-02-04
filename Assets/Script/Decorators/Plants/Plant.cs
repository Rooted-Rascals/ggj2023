using System;
using System.Collections.Generic;
using System.Linq;
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
    
    [Price(10)]
    public abstract class Plant : MonoBehaviour
    {
        [SerializeField] private float waterConsumption = 1f;
        [SerializeField] private float waterGeneration = 0f;
        [SerializeField] private float energyGeneration = 0f;
        public abstract PlantType PlantType { get; }

        public virtual void DoAction()
        {
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
        
        public List<AudioClip> GrowingSound => GetGrowingSounds();

        private List<AudioClip> _growingSound = null;
        
        private List<AudioClip> GetGrowingSounds()
        {
            return _growingSound ??= Resources.LoadAll<AudioClip>($"Sounds/{PlantType.ToString()}/Growing").ToList();
        }
    }
    
    public class PriceAttribute : Attribute
    {
        public int Price;
        public PriceAttribute(int price)
        {
            this.Price = price;
        }
    }
}