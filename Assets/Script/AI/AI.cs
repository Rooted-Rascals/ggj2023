using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AI : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float movementSpeedModifier = 1.0f;
    public float attackRange = 1f;
    public float attackDelay = 3f;
    public float attackDamage = 5f;
    private float attackCooldown = 0f;
    public UnityEvent<GameObject> deathTrigger = new UnityEvent<GameObject>();

    private void Update()
    {
        attackCooldown = Mathf.Max(0f, attackCooldown - Time.deltaTime);
    }

    
#if UNITY_EDITOR    
    public void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, Mathf.Sqrt(attackRange), 0.25f);
    }
#endif   

    public void MoveTo(Vector3 target)
    {
        float deltaX = target.x - transform.position.x;
        float deltaZ = target.z - transform.position.z;
        Vector3 direction = new Vector3(deltaX, 0f, deltaZ).normalized;
        transform.position += direction * (Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public Tile GetTileUnderneath()
    {
        RaycastHit hit;
        if (Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                500f,
                LayerMask.GetMask("Tiles")
            ))
        {
            return hit.collider.GetComponent<Tile>();
        }

        return null;
    }

    public bool IsInAttackRange(Vector3 target)
    {
        return Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2) <= attackRange;
    }

    public void Attack(GameObject target)
    {
        if (attackCooldown <= 0)
        {
            attackCooldown = attackDelay;
            // Trigger attack animation
            HealthManager health = target.GetComponent<HealthManager>();
            health.Damage(attackDamage);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        deathTrigger.Invoke(this.gameObject);
    }
}
