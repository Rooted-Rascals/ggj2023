using Script.Decorators;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ShopManager : MonoBehaviour
    {
        private Canvas _canvas;

        [SerializeField] private Button RootButton;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;

            MouseManager.Instance.onSelection.AddListener(OnSelection);
        }

        private void OnSelection(GameObject selection)
        {
            TileDecorator tileDecorator = selection?.GetComponentInChildren<TileDecorator>();
            
            if (selection is null || tileDecorator is null)
            {
                _canvas.enabled = false;
                return;
            }
            
            RootButton.gameObject.SetActive(tileDecorator.CanBuildRoots);
            _canvas.enabled = true;
        }
    }
}