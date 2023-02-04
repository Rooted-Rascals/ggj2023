using Script.Decorators.Biomes;
using Script.Decorators.Plants;
using Script.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class MotherTreeOrchestrator : MonoBehaviour
    {
        private float defaultWaterConsumption = 10f;
        private float defaultEnergyGeneration = 3f;
        private float defaultWaterGeneration = 3f;

        private HashSet<Tile> RootsList = new HashSet<Tile>();
        private HashSet<Tile> EnabledRootsList = new HashSet<Tile>();

        public void AddRoots(Tile tile)
        {
            RootsList.Add(tile);
            EnabledRootsList.Add(tile);

            UpdateRootsList();
        }

        public void RemoveRoots()
        {
            Tile tile = MouseManager.Instance.CurrentSelectedObject.GetComponent<Tile>();
            tile.SetRootsTile(null);

            UpdateRootsList();
        }

        private void UpdateRootsList()
        {
            //start
            Tile startTile = gameObject.GetComponentInParent<Tile>();

            Queue<Tile> toVisit = new Queue<Tile>();
            HashSet<Tile> visited = new HashSet<Tile>();
            List<Tile> neighbours = startTile.GetNeighboursTile();

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

                neighbours = startTile.GetNeighboursTile();

                foreach (Tile neighbour in neighbours)
                {
                    Biome biome = neighbour.GetComponentInChildren<Biome>();
                    if (biome && biome.hasRoots)
                    {
                        toVisit.Enqueue(neighbour);
                    }
                }
                visited.Add(next);
            }

            EnabledRootsList = visited;
        }

        public float GetEnergyGeneration()
        {
            return defaultEnergyGeneration; // + all attached object that generate energy
        }

        public float GetWaterConsumption()
        {
            float waterConsumption = defaultWaterConsumption;
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
            return defaultWaterGeneration;
        }
    }
}