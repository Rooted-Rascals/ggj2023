using System;
using Script.Decorators.Biomes;
using Script.Decorators.Plants;
using Script.Manager;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Script
{
    public class MotherTreeOrchestrator : MonoBehaviour
    {
        private float defaultWaterConsumption = 5f;
        private float defaultEnergyGeneration = 4f;
        private float defaultWaterGeneration = 4f;

        private HashSet<Tile> RootsList = new HashSet<Tile>();
        private HashSet<Tile> EnabledRootsList = new HashSet<Tile>();
        private HashSet<Tile> ConnectedPlant = new HashSet<Tile>();

        [SerializeField] private ParticleSystem ParticleSystem;

        public void Update()
        {
            foreach (Tile EnabledRoots in EnabledRootsList)
            {
                Plant plant = EnabledRoots.GetComponentInChildren<Plant>();
                if (plant)
                { 
                    plant.DoAction();
                }
            }
        }

        public void TriggerGeneration()
        {
            foreach (Tile EnabledRoots in EnabledRootsList)
            {
                Plant plant = EnabledRoots.GetComponentInChildren<Plant>();
                if (plant)
                {
                    plant.TriggerGeneration();
                }
            }
            ParticleSystem.Play();
        }

        public List<Tile> GetAllRoots()
        {
            return RootsList.ToList();
        }

        public int GetRootsCount() => RootsList.Count;

        public void AddRoots(Tile tile)
        {
            RootsList.Add(tile);
            EnabledRootsList.Add(tile);

            UpdateRootsList();
            ShowIsNotConnected();
        }

        public void RemoveRoots(Tile tile)
        {
            RootsList.Remove(tile);
            tile.SetRootsTile(null);
            UpdateRootsList();
            ShowIsNotConnected();
            tile.IsNotConnected.SetActive(false);
        }

        public bool IsRootConnected(Tile tile)
        {
            return EnabledRootsList.Contains(tile);
        }

        private void ShowIsNotConnected()
        {
            foreach (Tile tile in RootsList)
            {
                if (!IsRootConnected(tile))
                    tile.IsNotConnected.SetActive(true);
                else
                    tile.IsNotConnected.SetActive(false);
            }
        }

        public void UpdateRootsList()
        {
            //start
            Tile startTile = gameObject.GetComponentInParent<Tile>();

            Queue<Tile> toVisit = new Queue<Tile>();
            HashSet<Tile> visited = new HashSet<Tile>();
            List<Tile> neighbours = startTile.GetNeighboursTile();
            ConnectedPlant = new HashSet<Tile>();

            visited.Add(startTile);

            foreach (Tile neighbour in neighbours)
            {
                Biome biome = neighbour.GetComponentInChildren<Biome>();
                if(biome && biome.hasRoots)
                {
                    toVisit.Enqueue(neighbour);
                }
            }

            while (toVisit.Count > 0)
            {
                Tile next = toVisit.Dequeue();

                if (visited.Contains(next))
                    continue;

                neighbours = next.GetNeighboursTile();

                foreach (Tile neighbour in neighbours)
                {
                    Biome biome = neighbour.GetComponentInChildren<Biome>();
                    if (biome && biome.hasRoots)
                    {
                        toVisit.Enqueue(neighbour);
                        if (neighbour.CurrentBuilding != null)
                            ConnectedPlant.Add(neighbour);
                    }
                }
                visited.Add(next);
            }

            visited.Add(startTile);
            EnabledRootsList = visited;
        }

        public float GetEnergyGeneration()
        {
            float energyGeneration = 0f;

            foreach (Tile EnabledRoots in EnabledRootsList)
            {
                Plant plant = EnabledRoots.GetComponentInChildren<Plant>();
                if (plant)
                {
                    energyGeneration += plant.GetEnergyGeneration();
                }
            }
            return energyGeneration;
        }

        public float GetWaterConsumption()
        {
            float waterConsumption = 0f;
            foreach (Tile EnabledRoots in EnabledRootsList)
            {
                Plant plant = EnabledRoots.GetComponentInChildren<Plant>();
                if (plant)
                {
                    waterConsumption += plant.GetWaterConsumption();
                }
            }
            return waterConsumption;
        }

        public float GetWaterGeneration()
        {
            float waterGeneration = 0f;
            foreach (Tile EnabledRoots in EnabledRootsList)
            {
                Plant plant = EnabledRoots.GetComponentInChildren<Plant>();
                if (plant)
                {
                    waterGeneration += plant.GetWaterGeneration();
                }
            }
            return waterGeneration;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position + Vector3.up * 3f, EnabledRootsList.ToString());
        }
#endif
    }
}