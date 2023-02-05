using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image HealthBar;


    private HealthManager HealthManager;


    // Start is called before the first frame update
    void Start()
    {
        if (resourcesManager == null)
        {
            resourcesManager = FindObjectOfType<ResourcesManager>();
        }
    }

    public void Update()
    {
        if (HealthManager)
            HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, HealthManager.GetHealth() / HealthManager.GetMaxHealth(), 0.1f);
    }

    private void OnGUI()
    {
        energyCountText.text = $"{resourcesManager.GetEnergyCount()}";
        energyProductionText.text = $"+{GameManager.Instance.GetEnergyGeneration()}/s";
        waterCountText.text = $"{resourcesManager.GetWaterCount()}";
        float number = GameManager.Instance.GetWaterGeneration() - GameManager.Instance.GetWaterConsumption();
        waterProductionText.text = (number > 0?"+" + number: number) + "/s";
        healthText.text = $"{GameManager.Instance.GetMotherTree()?.GetComponent<HealthManager>()?.GetHealth() ?? 0}";
        timeText.text = $"{Mathf.RoundToInt(GameManager.Instance.GetTotalTime())}s";
        HealthManager = GameManager.Instance.GetMotherTree()?.GetComponent<HealthManager>();
    }
}
