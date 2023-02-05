using UnityEngine;

namespace Script.Decorators.Plants
{
    [Buyable(5, nameof(Leaf), "Amplify the rate of sun that you get.")]
    public class Leaf : Plant
    {
        public override PlantType PlantType => PlantType.LEAF;

        public override void TriggerGeneration()
        {
            StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
        }
    }
}