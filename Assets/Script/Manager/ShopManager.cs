using Script.Decorators.Plants;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ShopManager : MonoBehaviour
    {
        private Canvas _canvas;

        [SerializeField] private Button rootButton;
        [SerializeField] private Button cactusButton;
        [SerializeField] private Button lilyPadButton;
        [SerializeField] private Button mushroomButton;
        [SerializeField] private Button leafButton;
        
        private Tile _currentTile;
        
        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;

            MouseManager.Instance.onSelection.AddListener(OnSelection);
        }

        private void OnSelection(GameObject selection)
        {
            _currentTile = selection?.GetComponent<Tile>();
            
            if (_currentTile?.CurrentBiome is null)
            {
                _canvas.enabled = false;
                return;
            }
            
            rootButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildRoots);
            cactusButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildCactus);
            lilyPadButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildLilyPad);
            mushroomButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildMushroom);
            leafButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildLeaf);

            
            _canvas.enabled = true;
        }

        public void RefreshMenu() => OnSelection(_currentTile.gameObject);
        
        public void BuildRoots()
        {
            GetComponent<RootsSystem>().BuildRoots();
            _currentTile.CurrentBiome.hasRoots = true;

            RefreshMenu();
        }

        public void BuildCactus()
        {
            _currentTile.SetActiveBuildingTile(PlantType.CACTUS);
            
            RefreshMenu();
        }

        public void BuildLilyPad()
        {
            _currentTile.SetActiveBuildingTile(PlantType.LILYPAD);
            
            RefreshMenu();
        }
        
        public void BuildMushroom()
        {
            _currentTile.SetActiveBuildingTile(PlantType.MUSHROOM);
            
            RefreshMenu();
        }

        public void BuildLeaf()
        {
            _currentTile.SetActiveBuildingTile(PlantType.LEAF);
            
            RefreshMenu();
        }
    }
}