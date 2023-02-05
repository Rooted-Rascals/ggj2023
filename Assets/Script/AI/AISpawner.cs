using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField] private AI aiPrefab;
    
    public AI SpawnAI()
    {
        AI spawnedAI = Instantiate(aiPrefab, transform.position, Quaternion.identity);
        return spawnedAI;
    }

#if UnityEditor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
#endif
}
