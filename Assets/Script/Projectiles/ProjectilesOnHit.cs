using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesOnHit : MonoBehaviour
{
    [SerializeField]
    private float Damage = 20f;

    private float lifeSpan = 0.5f;
    [SerializeField] private bool decayActivated = true;

    private bool decayCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<AI>())
        {
            print("HIT");
            other.gameObject.GetComponent<HealthManager>().Damage(Damage);
            Destroy(gameObject);
        }

    }

    void Update()
    {
        if (decayCompleted || !decayActivated)
        {
            return;
        }

        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0)
        {
            decayCompleted = true;
            Destroy(gameObject);
        }
    }
}
