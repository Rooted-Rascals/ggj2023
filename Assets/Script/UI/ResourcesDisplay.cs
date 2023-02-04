using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI energyCountText;
    [SerializeField]
    private TextMeshProUGUI energyProductionText;
    [SerializeField]
    private TextMeshProUGUI waterCountText;
    [SerializeField]
    private TextMeshProUGUI waterProductionText;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private ResourcesManager resourcesManager;
    // Start is called before the first frame update
    void Start()
    {
        if (resourcesManager == null)
        {
            resourcesManager = FindObjectOfType<ResourcesManager>();
        }
    }

    private void OnGUI()
    {
        energyCountText.text = $"{resourcesManager.GetEnergyCount()}";
        energyProductionText.text = $"{GameManager.Instance.GetEnergyGeneration()}";
        waterCountText.text = $"{resourcesManager.GetWaterCount()}";
        waterProductionText.text = $"{GameManager.Instance.GetWaterGeneration() - GameManager.Instance.GetWaterConsumption()}";
        healthText.text = $"{GameManager.Instance.GetMotherTree().GetComponent<HealthManager>().GetHealth()}";
        timeText.text = $"{Mathf.RoundToInt(GameManager.Instance.GetTotalTime())}s";
    }
}
