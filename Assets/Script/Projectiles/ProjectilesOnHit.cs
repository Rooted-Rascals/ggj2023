using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesOnHit : MonoBehaviour
{
    [SerializeField]
    private float Damage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<AI>())
        {
            other.gameObject.GetComponent<HealthManager>().Damage(Damage);
            Destroy(this);
        }

    }
}
