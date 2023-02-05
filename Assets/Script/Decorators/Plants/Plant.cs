using System;
using System.Collections;
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
        bool isReady = false;
        public abstract PlantType PlantType { get; }

        public bool IsReady => isReady;


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
            StartCoroutine(SpawnBuilding(transform, new Vector3(1.1f, 1.3f, 1.1f), 0.38f, 0.09f));
        }

        private IEnumerator SpawnBuilding(Transform transform, Vector3 upScale, float duration1, float duration2)
        {
            Vector3 finalScale = transform.localScale;
            Vector3 initialScale = Vector3.zero;

            for (float time = 0; time < duration1; time += Time.deltaTime)
            {
                float progress = Mathf.PingPong(time, duration1) / duration1;
                transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
                yield return null;
            }

            for (float time = 0; time < duration2; time += Time.deltaTime)
            {
                float progress = Mathf.PingPong(time, duration2) / duration2; 
                transform.localScale = Vector3.Lerp(upScale, finalScale, progress);
                yield return null;
            }
            isReady = true;
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