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

        public virtual void TriggerGeneration()
        {
            return;
        }
        
        public List<AudioClip> GrowingSound => GetGrowingSounds();

        private List<AudioClip> _growingSound = null;
        
        private List<AudioClip> GetGrowingSounds()
        {
            return _growingSound ??= Resources.LoadAll<AudioClip>($"Sounds/{PlantType.ToString()}/Growing").ToList();
        }

        public void Start()
        {
            HealthManager buildingsHealth = gameObject.GetComponent<HealthManager>();
            buildingsHealth.onDeath.AddListener(Die);
        }

        public virtual void Die()
        {
            Tile tile = gameObject.GetComponentInParent<Tile>();
            GameManager.Instance.GetMotherTree().RemoveRoots(tile);
            Destroy(gameObject);
        }
    }

    public class BuyableAttribute : Attribute
    {
        public readonly int Price;
        public readonly string Description;
        public readonly string Name;
        
        public BuyableAttribute(int price, string name, string description)
        {
            Price = price;
            Name = name;
            Description = description;
        }
    }
}