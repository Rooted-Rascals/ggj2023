using System;
using System.Collections.Generic;
using System.Linq;
using Script.Decorators.Biomes;
using Script.Decorators.Plants;
using TMPro;
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
        private Dictionary<PlantType, int> _pricesCache;
        private Material GrayedOutMaterial;
        private string SunEmoticon = "<sprite index= 0>";

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            _audioSource = this.AddComponent<AudioSource>();
            GrayedOutMaterial = Resources.Load<Material>("Materail/Gray");
            MouseManager.Instance.onSelection.AddListener(OnSelection);

            CalculatePrices();
            PopulatePopupInfos();
        }

        private void PopulatePopupInfos()
        {
            rootButton.AddComponent<PopupInfo>().Panel.SetInfos("Roots", "Roots allow you to expand you network and explore the area. You can always build next to another root. The price scale the more you have roots.");
            cactusButton.AddComponent<PopupInfo>().Panel.SetInfos<Cactus>();
            leafButton.AddComponent<PopupInfo>().Panel.SetInfos<Leaf>();
            mushroomButton.AddComponent<PopupInfo>().Panel.SetInfos<Mushroom>();
            lilyPadButton.AddComponent<PopupInfo>().Panel.SetInfos<LilyPad>();
        }

        private void CalculatePrices()
        {
            _pricesCache = new Dictionary<PlantType, int>
            {
                { PlantType.CACTUS, typeof(Cactus).GetAttribute<BuyableAttribute>().Price },
                { PlantType.LEAF, typeof(Leaf).GetAttribute<BuyableAttribute>().Price },
                { PlantType.MUSHROOM, typeof(Mushroom).GetAttribute<BuyableAttribute>().Price },
                { PlantType.LILYPAD, typeof(LilyPad).GetAttribute<BuyableAttribute>().Price },
                { PlantType.MOTHERTREE, typeof(MotherTree).GetAttribute<BuyableAttribute>().Price }
            };

            cactusButton.GetComponentInChildren<TextMeshProUGUI>().text = _pricesCache[PlantType.CACTUS] + SunEmoticon;
            leafButton.GetComponentInChildren<TextMeshProUGUI>().text = _pricesCache[PlantType.LEAF] + SunEmoticon;
            mushroomButton.GetComponentInChildren<TextMeshProUGUI>().text = _pricesCache[PlantType.MUSHROOM] + SunEmoticon;
            lilyPadButton.GetComponentInChildren<TextMeshProUGUI>().text = _pricesCache[PlantType.LILYPAD] + SunEmoticon;
        }

        private void Update()
        {
            //If you can't buy, the button is gray.
            rootButton.image.material = !ResourcesManager.Instance.CanAfford(RootPrice) ? GrayedOutMaterial : null;
            leafButton.image.material = !ResourcesManager.Instance.CanAfford(_pricesCache[PlantType.LEAF]) ? GrayedOutMaterial : null;
            mushroomButton.image.material = !ResourcesManager.Instance.CanAfford(_pricesCache[PlantType.MUSHROOM]) ? GrayedOutMaterial : null;
            cactusButton.image.material = !ResourcesManager.Instance.CanAfford(_pricesCache[PlantType.CACTUS]) ? GrayedOutMaterial : null;
            lilyPadButton.image.material = !ResourcesManager.Instance.CanAfford(_pricesCache[PlantType.LILYPAD]) ? GrayedOutMaterial : null;
        }

        private void OnSelection(GameObject selection)
        {
            _currentTile = selection?.GetComponent<Tile>();
            
            if (_currentTile?.CurrentBiome is null)
            {
                _canvas.enabled = false;
                return;
            }
            
            rootButton.GetComponentInChildren<TextMeshProUGUI>().text = RootPrice + SunEmoticon;
            rootButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildRoots);
            cactusButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildCactus);
            lilyPadButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildLilyPad);
            mushroomButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildMushroom);
            leafButton.gameObject.SetActive(_currentTile.CurrentBiome.CanBuildLeaf);
            
            _canvas.enabled = true;
        }

        private void RefreshMenu() => OnSelection(_currentTile.gameObject);
        
        private int RootPrice => GameManager.Instance.GetMotherTree()?.GetRootsCount() ?? 0;
        
        public void BuildRoots()
        {
            if (!ResourcesManager.Instance.CanAfford(RootPrice))
                return;
            
            GetComponent<RootsSystem>().BuildRoots();
            ResourcesManager.Instance.DecreaseEnergyCount(RootPrice);
            _currentTile.CurrentBiome.hasRoots = true;
            _audioSource.PlayOneShot(_currentTile.CurrentBiome.RootsBuildSounds[UnityEngine.Random.Range(0, _currentTile.CurrentBiome.RootsBuildSounds.Count)]);
            
            RefreshMenu();
        }

        private void Build(PlantType type)
        {
            int price = _pricesCache[type];

            if (!ResourcesManager.Instance.CanAfford(price))
            {
                //Play error sound?
                return;    
            }
            
            _currentTile.SetActiveBuildingTile(type);
            ResourcesManager.Instance.DecreaseEnergyCount(price);
            _audioSource.PlayOneShot(_currentTile.CurrentBuilding.GrowingSound[UnityEngine.Random.Range(0, _currentTile.CurrentBuilding.GrowingSound.Count)]);
            StartCoroutine(Coroutines.SpawnBuilding(_currentTile.CurrentBuilding.gameObject.transform, new Vector3(1.1f, 1.3f, 1.1f), 0.6f, 0.15f));
            RefreshMenu();
        }

        public void BuildLeaf() => Build(PlantType.LEAF);
        
        public void BuildCactus() => Build(PlantType.CACTUS);

        public void BuildLilyPad() => Build(PlantType.LILYPAD);

        public void BuildMushroom() => Build(PlantType.MUSHROOM);
    }
}