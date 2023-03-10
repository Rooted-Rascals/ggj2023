using Script.Decorators.Biomes;
using Script.Decorators.Enemies;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AI : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float movementSpeedModifier = 1.0f;
    public float attackRange = 1f;
    public float agroRange = 3f;
    public float attackDelay = 3f;
    public float attackDamage = 5f;
    private float attackCooldown = 0f;
    public UnityEvent<GameObject> deathTrigger = new UnityEvent<GameObject>();
    private AudioClip deathSound;
    private AudioSource audioSource;
    private bool isDead = false;
    [SerializeField] private EnemiesType enemiesType;
    [SerializeField]
    private Animation meshAnimation;
    [SerializeField] private AnimationClip dieAnimation;
    [SerializeField] private AnimationClip attackAnimation;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.AddComponent<AudioSource>();
        } 
        deathSound = Resources.Load<AudioClip>($"Sounds/ENNEMY/{enemiesType.ToString()}_death");
        if (meshAnimation == null)
        {
            meshAnimation = GetComponentInChildren<Animation>();
        }
    }
    public float GetRange()
    {
        return attackRange;
    }
    
    private void Update()
    {
        if (isDead)
        {
            return;
        }
        attackCooldown = Mathf.Max(0f, attackCooldown - Time.deltaTime);
    }

    public void UpdateDifficulty(float difficulty)
    {
        HealthManager health = GetComponent<HealthManager>();
        health.UpdateMaxHealth(health.MaxHealth * difficulty);
        attackDamage = attackDamage * difficulty;
    }

    public EnemiesType EnemiesType => enemiesType;
    
#if UNITY_EDITOR    
    public void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, Mathf.Sqrt(attackRange), 0.25f);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, Mathf.Sqrt(agroRange), 0.25f);
    }
#endif   

    public void MoveTo(Vector3 target)
    {
        if (isDead)
        {
            return;
        }
        float deltaX = target.x - transform.position.x;
        float deltaZ = target.z - transform.position.z;
        Vector3 direction = new Vector3(deltaX, 0f, deltaZ).normalized;
        transform.position += direction * (Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.LookRotation(direction);
    }



    public bool IsInAgroRange(Vector3 target)
    {
        return Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2) <= agroRange;
    }

    public bool IsInAttackRange(Vector3 target)
    {
        return Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2) <= attackRange;
    }

    public void Attack(GameObject target)
    {
        if (isDead)
        {
            return;
        }
        float deltaX = target.transform.position.x - transform.position.x;
        float deltaZ = target.transform.position.z - transform.position.z;
        Vector3 direction = new Vector3(deltaX, 0f, deltaZ).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        if (attackCooldown <= 0)
        {
            attackCooldown = attackDelay;
            meshAnimation.Stop();
            meshAnimation.clip = attackAnimation;
            meshAnimation.Play();
            HealthManager health = target.GetComponent<HealthManager>();
            health.Damage(attackDamage);
        }

    }

    public void Kill()
    {
        audioSource.PlayOneShot(deathSound);
        isDead = true;
        meshAnimation.Stop();
        meshAnimation.clip = dieAnimation;
        meshAnimation.Play();
        Destroy(gameObject, Mathf.Max(dieAnimation.length, deathSound.length));
    }

    public void OnDestroy()
    {
        deathTrigger.Invoke(this.gameObject);
    }
}
