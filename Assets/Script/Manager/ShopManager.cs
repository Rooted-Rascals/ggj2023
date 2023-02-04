using Script.Decorators;
using Script.Decorators.Buildings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ShopManager : MonoBehaviour
    {
        private Canvas _canvas;

        [SerializeField] private Button RootButton;
        [SerializeField] private Button CactusButton;


        private Tile currentTile;
        
        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;

            MouseManager.Instance.onSelection.AddListener(OnSelection);
        }

        private void OnSelection(GameObject selection)
        {
            currentTile = selection?.GetComponent<Tile>();
            
            if (currentTile?.CurrentTyleDecorator is null)
            {
                _canvas.enabled = false;
                return;
            }
            
            RootButton.gameObject.SetActive(currentTile.CurrentTyleDecorator.CanBuildRoots);
            CactusButton.gameObject.SetActive(currentTile.CurrentTyleDecorator.CanBuildCactus);
            _canvas.enabled = true;
        }

        public void RefreshMenu() => OnSelection(currentTile.gameObject);
        
        public void BuildRoots()
        {
            GetComponent<RootsSystem>().BuildRoots();
            currentTile.CurrentTyleDecorator.hasRoots = true;

            RefreshMenu();
        }

        public void BuildCactus()
        {
            currentTile.SetActiveBuildingTile(TileBuildingType.CACTUS);
            
            RefreshMenu();
        }
    }
}