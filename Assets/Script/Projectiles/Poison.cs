using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField] private float poisonTickDelay = 3f;

    [SerializeField] private float poisonTickDmg = 5f;

    private float cooldown = 0;
    private List<Collider> collidingObject = new List<Collider>();

    private Collider collider;
    void Start()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER IN");
        collidingObject.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("TRIGGER OUT");
        collidingObject.Remove(other);
    }

    void Update()
    {
        if (cooldown <= 0)
        {
            foreach (Collider collider in collidingObject)
            {
                Debug.Log(this.collider);
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
}
