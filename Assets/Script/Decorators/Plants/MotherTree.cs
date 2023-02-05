using UnityEngine;

namespace Script.Decorators.Plants
{
    [Buyable(999999, nameof(MotherTree), "Don't let the mother tree die!")]
    public class MotherTree : Plant
    {
        public override PlantType PlantType => PlantType.MOTHERTREE;
        
        public override void TriggerGeneration()
        {
            StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
        }

        public override void Die()
        {
            GameManager.Instance.EndGame();
        }
    }
}