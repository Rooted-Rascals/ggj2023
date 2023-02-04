using System;
using System.Linq;
using Script.Decorators.Plants;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ShopManager : MonoBehaviour
    {
        private Canvas _canvas;
        private AudioSource _audioSource;
        
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
            _audioSource = this.AddComponent<AudioSource>();

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

            _audioSource.PlayOneShot(_currentTile.CurrentBiome.RootsBuildSounds[UnityEngine.Random.Range(0, _currentTile.CurrentBiome.RootsBuildSounds.Count)]);
            
            RefreshMenu();
        }

        private void Build(PlantType type)
        {
            _currentTile.SetActiveBuildingTile(type);
            _audioSource.PlayOneShot(_currentTile.CurrentBuilding.GrowingSound[UnityEngine.Random.Range(0, _currentTile.CurrentBuilding.GrowingSound.Count)]);
            RefreshMenu();
        }

        public void BuildLeaf() => Build(PlantType.LEAF);
        
        public void BuildCactus() => Build(PlantType.CACTUS);

        public void BuildLilyPad() => Build(PlantType.LILYPAD);

        public void BuildMushroom() => Build(PlantType.MUSHROOM);
    }
}