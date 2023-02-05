using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField]
    private float range = 3f;
    [SerializeField]
    private float fireRate = 5f;

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
    private float cooldown = 3f;

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
    }

    public void Update()
    {
        cooldown = Mathf.Max(0f, cooldown - Time.deltaTime);
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, range);

        float shortestDistance = range;
        Target = null;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<AI>())
            {
                float distanceEnemy = Vector3.Distance(new Vector3(hitCollider.transform.position.x, 0, hitCollider.transform.position.z),
                new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z));
                if (distanceEnemy < shortestDistance)
                {
                    shortestDistance = distanceEnemy;
                    Target = hitCollider.gameObject;
                }
            }
        }
    }

    public void ActionUpdate()
    {
        if (cooldown <= 0)
        {
            LaunchProjectile();
            cooldown = fireRate;
        }
    }
    
    // Update is called once per frame
    void LaunchProjectile()
    {
        if(Target != null)
        {
            StartCoroutine(Coroutines.ScaleUpAndDown(gameObject.transform, new Vector3(0.9f, 1.1f, 0.9f), 0.2f));
            Vector3 direction = (new Vector3(Target.transform.position.x ,0, Target.transform.position.z) - 
                new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z)).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            if (audioSource)
            {
                audioSource.PlayOneShot(audioClip, 0.25f);
            }
            GameObject instance = Instantiate(bullet, gameObject.transform.position + SpawnPosition, rotation);
            instance.GetComponent<Rigidbody>().velocity = direction * Speed;
            cooldown = fireRate;
        }
    }
}
