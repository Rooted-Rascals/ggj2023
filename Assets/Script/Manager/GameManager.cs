using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Script;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO : Change this for the real mother tree object
    public MotherTree motherTree = new MotherTree();

    private float totalTime = 0f;
    private float waitingTime = 0f;
    private float waterCount = 100f;
    private float energyCount = 0f;
    [SerializeField] private float delayBetweenResourcesGain = 3f;
    [SerializeField] private float productionMultiplier = 1f;
    [SerializeField] private float waterConsumptionMultiplier = 1f;
    [SerializeField] private float waterGenerationMultiplier = 1f;
    [SerializeField] private bool activated = true;
    [SerializeField] private bool isDebugLogging = false;

    void Start()
    {
        waitingTime = 0f;
        waterCount = 100f;
        energyCount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            return;
        }

        UpdateResources();
        UpdateTreeStats();
    }

    private void UpdateResources()
    {
        totalTime += Time.deltaTime;
        waitingTime += Time.deltaTime;

        if (waitingTime >= delayBetweenResourcesGain)
        {
            waitingTime = -1f;
            GainResources();
            if (isDebugLogging)
            {
                Debug.Log($"E : {energyCount} ; W : {waterCount}");
            }
        }
    }

    private void GainResources()
    {
        if (motherTree == null)
        {
            return;
        }

        energyCount += GetEnergyGeneration();
        waterCount -= GetEnergyConsumption();
        waterCount += GetWaterGeneration();

        if (waterCount < 0)
        {
            waterCount = 0;
        }
    }

    private void UpdateTreeStats()
    {
        if (waterCount <= 0)
        {
            // TODO : Hurt tree life
        }
    }

    public bool VerifyIfEnoughEnergy(float cost)
    {
        return cost <= energyCount;
    }

    public bool ReduceEnergy(float energy)
    {
        if (VerifyIfEnoughEnergy(energy))
        {
            energyCount -= energy;
            return true;
        }

        return false;
    }

    public float GetEnergyConsumption()
    {
        return motherTree.GetWaterConsumption() * waterConsumptionMultiplier;
    }

    public float GetEnergyGeneration()
    {
        return motherTree.GetEnergyGeneration() * productionMultiplier;
    }

    public float GetWaterGeneration()
    {
        return motherTree.GetWaterGeneration() * waterGenerationMultiplier;
    }

    public float GetWaterCount()
    {
        return waterCount;
    }

    public float GetEnergyCount()
    {
        return energyCount;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }
}