using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.Decorators.Plants
{
    public enum PlantType
    {
        NONE,
        MOTHERTREE,
        CACTUS,
        LILYPAD,
        MUSHROOM
    };
    
    public abstract class Plant : MonoBehaviour
    {
        public abstract PlantType PlantType { get; }

        public virtual float WaterRequirement { get; } = 1f;

        public List<AudioClip> GrowingSound => GetGrowingSounds();

        private List<AudioClip> _growingSound = null;
        
        private List<AudioClip> GetGrowingSounds()
        {
            return _growingSound ??= Resources.LoadAll<AudioClip>($"Sounds/{PlantType.ToString()}/Growing").ToList();
        }
    }
}