using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Decorators
{
    public abstract class TileDecorator : MonoBehaviour
    {
        public abstract TileType Type { get; }

        #region Roots

        public bool hasRoots = false;
        public virtual bool CanBuildRoots => !hasRoots;
        public void BuildRoots()
        {
            
        }

        #endregion
        
    }
}
