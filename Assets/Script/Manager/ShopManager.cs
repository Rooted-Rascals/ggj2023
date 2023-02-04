using System;
using Script.Decorators;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Manager
{
    public class ShopManager : MonoBehaviour
    {
        private Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;

            MouseManager.Instance.onSelection.AddListener(OnSelection);
        }

        private void OnSelection(GameObject selection)
        {
            TileDecorator tileDecorator = selection.GetComponent<TileDecorator>();
            
        }
    }
}