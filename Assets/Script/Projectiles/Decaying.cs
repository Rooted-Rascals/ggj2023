using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Decaying : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 10f;

    [SerializeField] private bool decayActivated = true;

    public UnityEvent<GameObject> completelyDecay = new UnityEvent<GameObject>();

    private bool decayCompleted = false;

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
            completelyDecay.Invoke(gameObject);
        }
    }
}