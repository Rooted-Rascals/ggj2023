using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WaterPriceText;
    [SerializeField] private TextMeshProUGUI SunPriceText;

    private const string Emoticon = "<sprite index= 0>";

    public void SetValues(int sunPrice, float waterPrice)
    {
        WaterPriceText.text = waterPrice + Emoticon;
        SunPriceText.text = sunPrice + Emoticon;
    }
}
