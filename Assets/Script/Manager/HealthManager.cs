using System;
using System.Collections;
using System.Collections.Generic;
using Script.Decorators.Plants;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
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


    GameObject HealthBar;

    private bool isDead = false;

    private void Awake()
    {
        Health = MaxHealth;
        Vector3 healthBarPosition = new Vector3(transform.position.x, Height, transform.position.z);
        HealthBar = Instantiate(Resources.Load("Prefab/HealthBar"),  healthBarPosition, Quaternion.identity) as GameObject;
        HealthBar.transform.parent = gameObject.transform;
        HealthBarImage = HealthBar.GetComponentInChildren<Image>();
        if (isEnnemy)
            HealthBarImage.color = Color.red;
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
        if(Health < 0 && !isDead)
        {
            Health = 0;
            isDead = true;
            onDeath.Invoke();
        }
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
        MaxHealth = newMaxHealth;
    }

    public float GetHealth()
    {
        return Health;
    }
}
