using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Decorators
{

    public abstract class TileDecorator : MonoBehaviour
    {
        public abstract TileType Type { get; }

        #region Roots

        public bool hasRoots = false;

        public List<RootsType> RootsList = new();
        public virtual bool CanBuildRoots => !hasRoots;

        #endregion

        public bool hasCactus = false;



        
        

    }
}
