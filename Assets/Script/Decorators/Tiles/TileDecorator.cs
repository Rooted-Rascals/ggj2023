using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Decorators
{
    public abstract class TileDecorator : MonoBehaviour
    {
        public abstract TileType Type { get; }
        public bool IsVisible { get; set; } = false;
        
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
    }
}
