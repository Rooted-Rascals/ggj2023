using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Decorators
{
    public abstract class TileDecorator : MonoBehaviour
    {
        public abstract TileType Type { get; }

        public bool hasMotherTree = false;

        
        #region Roots

        public bool hasRoots = false;
        public virtual bool CanBuildRoots => !hasRoots && 
            RootsSystem.CheckNeighboursForRoots(gameObject.GetComponentInParent<Tile>());

        public List<RootsType> RootsList = new List<RootsType>();

        #endregion

        public bool hasCactus = false;

        
    }
}
