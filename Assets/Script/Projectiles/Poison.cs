using System;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField] private float poisonTickDelay = 3f;

    [SerializeField] private float poisonTickDmg = 5f;
    [SerializeField] private float radius = 2f;

    private float cooldown = 0;

    private Collider collider;
    void Start()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider>();
        }
    }
    void Update()
    {
        if (cooldown <= 0)
        {
            Collider[] collidingObject = Physics.OverlapSphere(transform.position,  radius);
            foreach (Collider collider in collidingObject)
            {
                if (collider.GetComponent<AI>())
                {
                    HealthManager enemyHealth = collider.GetComponent<HealthManager>();
                    enemyHealth.Damage(poisonTickDmg);
                }
            }

            cooldown = poisonTickDelay;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif

}
