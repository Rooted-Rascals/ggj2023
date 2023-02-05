using System.Collections.Generic;
using System.Linq;
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
            RootsBuildSounds = Resources.LoadAll<AudioClip>("Sounds/roots/Growing").ToList();
        }

        public abstract BiomeType Type { get; }
        public bool IsVisible { get; set; } = false;

        protected bool HasAPlant => CurrentPlantType != PlantType.NONE;
        private PlantType CurrentPlantType => _ownerTile.CurrentBuilding?.PlantType ?? PlantType.NONE;
        
        #region Roots
        
        public bool hasRoots = false;
        public virtual bool CanBuildRoots => !hasRoots && 
            RootsSystem.CheckNeighboursForRoots(gameObject.GetComponentInParent<Tile>()) &&
            IsVisible && !gameObject.GetComponentInParent<Tile>().IsNotConnected.activeSelf;

        public List<AudioClip> RootsBuildSounds;

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
        
        #region Leaf
        public virtual bool CanBuildLeaf => hasRoots && !HasAPlant;
        
        #endregion
    }
}
