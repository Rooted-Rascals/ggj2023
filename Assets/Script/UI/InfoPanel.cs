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
    private const string Emoticon = "<sprite index= 0>";

    public void SetInfos<T>() where T : Plant, new()
    {
        BuyableAttribute att = typeof(T).GetAttribute<BuyableAttribute>();
        
        Plant plant = Resources.Load<GameObject>($"Buildings/{typeof(T).Name}").transform.GetComponent<Plant>();
        Title.text = att.Name;
        Description.text = att.Description;
        
        SunProduction.text = "Sun production : " + plant.GetEnergyGeneration() + " " + Emoticon;
        WaterProduction.text = "Water production : " + plant.GetWaterGeneration() + " " + Emoticon;
        WaterConsomation.text = "Water consumption :" + plant.GetWaterConsumption() + " " + Emoticon;
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
