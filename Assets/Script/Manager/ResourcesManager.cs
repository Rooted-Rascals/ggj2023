using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesManager : MonoBehaviour
{
    private static ResourcesManager _instance = null;
    public static ResourcesManager Instance => _instance;
    
    private float waterCount = 100f;
    private float energyCount = 0f;
    public UnityEvent<float> energyChangeEvent = new UnityEvent<float>();

    public void Awake()
    {
        _instance = this;
    }

    public float GetEnergyCount()
    {
        return energyCount;
    }

    public float GetWaterCount()
    {
        return waterCount;
    }

    public bool CanAfford(float energyPrice)
    {
        return energyPrice <= energyCount;
    }
    
    public float IncreaseWaterCount(float additionalWater)
    {
        waterCount += additionalWater;
        return waterCount;
    }

    public float DecreaseWaterCount(float consumedWater)
    {
        waterCount = Mathf.Max(0f, waterCount - consumedWater);
        return waterCount;
    }

    public float IncreaseEnergyCount(float additionalEnergy)
    {
        energyCount += additionalEnergy;
        energyChangeEvent.Invoke(additionalEnergy);
        return energyCount;
    }
 
    public float DecreaseEnergyCount(float consumedEnergy)
    {
        energyCount = Mathf.Max(0f, energyCount - consumedEnergy);
        energyChangeEvent.Invoke(consumedEnergy);
        return energyCount;
    }
}
