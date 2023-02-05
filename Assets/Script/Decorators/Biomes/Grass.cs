using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Decorators.Biomes
{
    public class Grass : Biome
    {
        public override BiomeType Type => BiomeType.Grass;
        public override bool CanBuildMushroom => hasRoots && !HasAPlant;

        
        public new void Awake()
        {
            foreach (Transform grass in transform.GetComponentsInChildren<Transform>())
            {
                if(grass.name.StartsWith("random"))
                    grass.gameObject.SetActive(Random.Range(0, 3) == 0);
            }
            
            base.Awake();
        }
    }
}