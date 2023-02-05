using System;
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

    private void Start()
    {
        StartCoroutine(Coroutines.SpawnScalingUpAndDecay(gameObject.transform, lifeSpan, 0.5f));
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
            completelyDecay.Invoke(gameObject);
            Destroy(this.gameObject);
        }
    }
}
