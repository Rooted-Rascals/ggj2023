using System;
using UnityEditor;
using UnityEngine;

namespace Script.Decorators.Plants
{
    [Buyable(10, nameof(Mushroom), "Creates a cloud of toxic fumes that do damage over time to enemies in the area.")]
    public class Mushroom : Plant
    {

        [SerializeField] private float poisonTriggerDelay = 15f;
        [SerializeField]
        private GameObject poisonPatch;

        [SerializeField] private float detectionRadius = 1.5f;
        private float cooldown = 5f;
        [SerializeField]
        private AudioClip audioClip;

        public override PlantType PlantType => PlantType.MUSHROOM;

        void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (!IsReady)
            {
                return;
            }
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                cooldown = 0;
            }
        }

        public override void DoAction()
        {
            if (!IsReady)
            {
                return;
            }
            if (cooldown <= 0)
            {
                bool aiCollision = false;
                Collider[] collidingObject = Physics.OverlapSphere(transform.position, detectionRadius);
                foreach (Collider collider in collidingObject)
                {
                    if (collider.GetComponent<AI>())
                    {
                        aiCollision = true;
                        break;
                    }
                }

                if (!aiCollision)
                {
                    return;
                }

                StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
                cooldown = poisonTriggerDelay;
                Instantiate(poisonPatch, transform.position + Vector3.up * 1f, Quaternion.identity);
                audioSource.PlayOneShot(audioClip, 0.25f);
            }
        }
    }
}