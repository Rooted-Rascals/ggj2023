using System.Collections.Generic;
using Script.Decorators.Plants;
using UnityEngine;

namespace Script.Decorators.Biomes
{
    public abstract class Biome : MonoBehaviour
    {
        private Tile _ownerTile;
        
        public void Awake()
        {
            _ownerTile = GetComponentInParent<Tile>();
        }

        public abstract BiomeType Type { get; }
        public bool IsVisible { get; set; } = false;

        protected bool HasAPlant => CurrentPlantType != PlantType.NONE;
        private PlantType CurrentPlantType => _ownerTile.CurrentBuilding?.PlantType ?? PlantType.NONE;
        
        #region Roots

        public bool hasRoots = false;
        public virtual bool CanBuildRoots => !hasRoots && 
            RootsSystem.CheckNeighboursForRoots(gameObject.GetComponentInParent<Tile>()) &&
            IsVisible;

        public List<RootsType> RootsList = new List<RootsType>();

        #endregion

        #region Cactus

        public virtual bool CanBuildCactus => false;

        #endregion
        
        #region LilyPad

        public virtual bool CanBuildLilyPad => false;

        #endregion
        
        #region Mushroom
        public virtual bool CanBuildMushroom => false;

        #endregion
    }
}
