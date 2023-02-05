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
    
    public void SetInfos<T>() where T : Plant
    {
        BuyableAttribute att = typeof(T).GetAttribute<BuyableAttribute>();
        Title.text = att.Name;
        Description.text = att.Description;
    }

    public void SetInfos(string title, string description)
    {
        Title.text = title;
        Description.text = description;
    }
}
