using UnityEngine;

namespace Script.Decorators.Plants
{
    [Buyable(15, nameof(Leaf), "Amplify the rate of sun that you get.")]
    public class Leaf : Plant
    {
        public override PlantType PlantType => PlantType.LEAF;
        [SerializeField] private ParticleSystem ParticleSystem;

        public override void TriggerGeneration()
        {
            if (!IsReady)
            {
                return;
            }
            StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
            ParticleSystem.Play();
        }
    }
}