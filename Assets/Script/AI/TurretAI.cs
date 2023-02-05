using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField]
    private float range = 3f;
    [SerializeField]
    private float rate = 5f;
    [SerializeField]
    private float Speed = 8f;
    private SphereCollider Collider;
    [SerializeField]
    private Vector3 SpawnPosition;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject Target = null;
    [SerializeField]
    private float lifetime = 1.0f;

    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;
    

    

    void Start()
    {
        if(Collider is null)
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = range;
            Collider = sphereCollider;
        }

        if (audioSource is null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        InvokeRepeating("LaunchProjectile", 3f, 1f);
    }

    public void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, range);
        float shortestDistance = range;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<AI>() && Target == null)
            {
                float distanceEnemy = Vector3.Distance(transform.position, hitCollider.transform.position);
                if(distanceEnemy < shortestDistance)
                {
                    shortestDistance = distanceEnemy;
                    Target = hitCollider.gameObject;
                }
            }
        }
    }

    // Update is called once per frame
    void LaunchProjectile()
    {
        if(Target != null)
        {
            StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
            Vector3 direction = new Vector3(Target.transform.position.x ,0, Target.transform.position.z) - 
                new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(direction);
            if (audioSource)
            {
                audioSource.PlayOneShot(audioClip, 0.25f);
            }
            GameObject instance = Instantiate(bullet, gameObject.transform.position + SpawnPosition, rotation);
            instance.GetComponent<Rigidbody>().velocity = direction * Speed;
        }
    }
}
