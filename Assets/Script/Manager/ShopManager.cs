using Script.Decorators.Plants;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ShopManager : MonoBehaviour
    {
        private Canvas _canvas;

        [SerializeField] private Button RootButton;
        [SerializeField] private Button CactusButton;
        [SerializeField] private Button LilyPadButton;
        
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
            
            if (currentTile?.CurrentBiome is null)
            {
                _canvas.enabled = false;
                return;
            }
            
            RootButton.gameObject.SetActive(currentTile.CurrentBiome.CanBuildRoots);
            CactusButton.gameObject.SetActive(currentTile.CurrentBiome.CanBuildCactus);
            LilyPadButton.gameObject.SetActive(currentTile.CurrentBiome.CanBuildLilyPad);

            _canvas.enabled = true;
        }

        public void RefreshMenu() => OnSelection(currentTile.gameObject);
        
        public void BuildRoots()
        {
            GetComponent<RootsSystem>().BuildRoots();
            currentTile.CurrentBiome.hasRoots = true;

            RefreshMenu();
        }

        public void BuildCactus()
        {
            currentTile.SetActiveBuildingTile(PlantType.CACTUS);
            
            RefreshMenu();
        }

        public void BuildLilyPad()
        {
            currentTile.SetActiveBuildingTile(PlantType.LILYPAD);
            
            RefreshMenu();
        }
    }
}