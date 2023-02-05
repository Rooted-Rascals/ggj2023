using System;
using System.Collections;
using Script.Decorators.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private Image HealthBarImage;

    [SerializeField]
    public float MaxHealth = 100f;
    
    [SerializeField]
    private float Health;
    [SerializeField]
    private float LerpSpeed;

    [SerializeField]
    private bool isEnnemy = false;

    [SerializeField]
    private float Height = 3.0f;

    public UnityEvent onDeath;
    public UnityEvent onDamage;


    private AudioSource _audioSource;
    private AudioClip hitSound;
    private AudioClip deathSound;

    
    GameObject HealthBar;

    private bool isDying = false;
    private bool isDead = false;

    private void Awake()
    {
        Health = MaxHealth;
        Vector3 healthBarPosition = new Vector3(transform.position.x, Height, transform.position.z);
        HealthBar = Instantiate(Resources.Load("Prefab/HealthBar"),  healthBarPosition, Quaternion.identity) as GameObject;
        HealthBar.transform.parent = gameObject.transform;
        HealthBarImage = HealthBar.GetComponentInChildren<Image>();
        if (isEnnemy)
        {
            HealthBarImage.color = Color.red;
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = this.AddComponent<AudioSource>();
        }
        _audioSource.volume = 0.15f;
        hitSound = Resources.Load<AudioClip>("Sounds/ENNEMY/hit");
    }

    private void Update()
    {
        if(Health > MaxHealth)
            Health = MaxHealth;

        LerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();

    }

    public void HealthBarFiller()
    {
        HealthBarImage.fillAmount = Mathf.Lerp(HealthBarImage.fillAmount, Health / MaxHealth, LerpSpeed);
    }


    public void Damage(float damage)
    {
        onDamage.Invoke();
        Health -= damage;
        
        
        if(Health < 0 && !isDead && !isDying)
        {
            Die();
        }

        if (isDying || isDead)
            Health = 0;
        
        if(!isDead && !isDying)
            _audioSource.PlayOneShot(hitSound);
    }

    void Die()
    {
        isDying = true;
        Health = 0;
        onDeath.Invoke();
        isDead = true;
    }

    public void Heal(float healingPoint)
    {
        Health += healingPoint;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void UpdateMaxHealth(float newMaxHealth)
    {
        Health = newMaxHealth;
        MaxHealth = newMaxHealth;
    }

    public float GetHealth()
    {
        return Health;
    }
}
