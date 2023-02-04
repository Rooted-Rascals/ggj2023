using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Script;
using UnityEditor.Search;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager INSTANCE;

    public GameManager Instance
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
    [SerializeField] MotherTree motherTreePrefab;
    private MotherTree motherTree;

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

        UpdateResources();
        UpdateTreeStats();
    }

    private void Initialize()
    {
        waitingTime = 0f;
        Tile spawnTile = tilesManager.GetTile(new Vector3Int(0, 0, 0));
        motherTree = Instantiate(motherTreePrefab, spawnTile.transform.position, Quaternion.identity);
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
            // TODO : Hurt tree life
        }
    }

    public float GetWaterConsumption()
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

    public float GetTotalTime()
    {
        return totalTime;
    }
}