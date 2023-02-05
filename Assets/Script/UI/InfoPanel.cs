using System;
using System.Collections;
using System.Collections.Generic;
using Script.Decorators.Plants;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI SunProduction;
    [SerializeField] private TextMeshProUGUI WaterProduction;
    [SerializeField] private TextMeshProUGUI WaterConsomation;

    public void SetInfos<T>() where T : Plant, new()
    {
        BuyableAttribute att = typeof(T).GetAttribute<BuyableAttribute>();

        Plant plant = new T();
        Title.text = att.Name;
        SunProduction.text = "Sun production : " + plant.GetEnergyGeneration();;
        WaterProduction.text = "Water production : " + plant.GetWaterGeneration();
        WaterConsomation.text = "Water production :" + plant.GetWaterConsumption();
    }

    public void SetInfos(string title, string description)
    {
        Title.text = title;
        Description.text = description;

        SunProduction.text = String.Empty;
        WaterProduction.text = String.Empty;
        WaterConsomation.text = String.Empty;
    }
}
