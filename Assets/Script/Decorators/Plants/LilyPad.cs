using UnityEngine;

namespace Script.Decorators.Plants
{
    [Buyable(8, nameof(LilyPad), "Helps you gain water from water tiles.")]
    public class LilyPad : Plant
    {
        public override PlantType PlantType => PlantType.LILYPAD;
        
        public override void TriggerGeneration()
        {
            StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
        }
        
    }
}