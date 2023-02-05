using System;
using UnityEditor;
using UnityEngine;

namespace Script.Decorators.Plants
{
    [Price(6)]
    public class Mushroom : Plant
    {

        [SerializeField] private float poisonTriggerDelay = 15f;
        [SerializeField]
        private GameObject poisonPatch;
        private float cooldown = 5f;

        public override PlantType PlantType => PlantType.MUSHROOM;

        private void Update()
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                cooldown = 0;
            }
        }

        public override void DoAction()
        {
            if (cooldown <= 0)
            {
                StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
                cooldown = poisonTriggerDelay;
                Instantiate(poisonPatch, transform.position + Vector3.up * 1f, Quaternion.identity);
            }
        }
    }
}