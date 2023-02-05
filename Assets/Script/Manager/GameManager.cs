using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Script;
using Script.Decorators.Plants;
using UnityEditor.Search;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager INSTANCE;

    public static GameManager Instance
    {
        get
        {
            return INSTANCE;
        }
    }

    private float totalTime = 0f;
    private float waitingTime = 0f;
    [SerializeField] private float delayBetweenResourcesGain = 3f;
    [SerializeField] private float productionMultiplier = 1f;
    [SerializeField] private float waterConsumptionMultiplier = 1f;
    [SerializeField] private float waterGenerationMultiplier = 1f;
    [SerializeField] private bool activated = true;
    [SerializeField] private bool isDebugLogging = false;
    [SerializeField] private TilesManager tilesManager;
    [SerializeField] private ResourcesManager resourcesManager;
    [SerializeField] private float decayDamage = 3f;
    private MotherTreeOrchestrator motherTree;

    public MotherTreeOrchestrator GetMotherTree()
    {
        return motherTree;
    }

    void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else if (INSTANCE != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (!activated)
        {
            return;
        }

        totalTime += 0f;
        
        UpdateResources();
        UpdateTreeStats();
    }

    private void Initialize()
    {
        waitingTime = 0f;
        Tile spawnTile = tilesManager.GetTile(new Vector3Int(0, 0, 0));
        
        spawnTile.GetComponent<Tile>().SetActiveTile(BiomeType.Grass, false);
        GameObject spawnedObject = spawnTile.SetActiveBuildingTile(PlantType.MOTHERTREE);
        if (spawnedObject != null)
        {
            motherTree = spawnedObject.GetComponent<MotherTreeOrchestrator>();   
        }
        AIController.Instance.UpdateAIGrid();
    }

    private void UpdateResources()
    {
        totalTime += Time.deltaTime;
        waitingTime += Time.deltaTime;

        if (waitingTime >= delayBetweenResourcesGain)
        {
            waitingTime = 0f;
            GainResources();
        }
    }

    private void GainResources()
    {
        if (motherTree == null)
        {
            return;
        }

        motherTree.TriggerGeneration();
        float energyCount = resourcesManager.IncreaseEnergyCount(GetEnergyGeneration());
        resourcesManager.IncreaseWaterCount(GetWaterGeneration());
        float waterCount = resourcesManager.DecreaseWaterCount(GetWaterConsumption());
        if (isDebugLogging)
        {
            Debug.Log($"E : {energyCount} ; W : {waterCount}");
        }
    }

    private void UpdateTreeStats()
    {
        if (resourcesManager.GetWaterCount() <= 0)
        {
            HealthManager health = motherTree.GetComponent<HealthManager>();
            health.Damage(decayDamage);
        }
    }

    public float GetWaterConsumption()
    {
        if (motherTree == null)
        {
            return 0f;
        }
        return motherTree.GetWaterConsumption() * waterConsumptionMultiplier;
    }

    public float GetEnergyGeneration()
    {
        if (motherTree == null)
        {
            return 0f;
        }
        return motherTree.GetEnergyGeneration() * productionMultiplier;
    }

    public float GetWaterGeneration()
    {
        if (motherTree == null)
        {
            return 0f;
        }
        return motherTree.GetWaterGeneration() * waterGenerationMultiplier;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }
}