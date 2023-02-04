using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private Image HealthBarImage;

    [SerializeField]
    private float Health, MaxHealth = 100f;
    [SerializeField]
    private float LerpSpeed;

    [SerializeField]
    private bool isEnnemy = false;

    [SerializeField]
    private float Height = 3.0f;

    public UnityEvent onDeath;
    GameObject HealthBar;

    private void Awake()
    {
        Health = MaxHealth;
        if (isEnnemy)
            HealthBarImage.color = Color.red;
        HealthBar = Instantiate(Resources.Load("Prefab/HealthBar"), new Vector3(0,Height,0), Quaternion.identity) as GameObject;
        HealthBar.transform.parent = gameObject.transform;
        HealthBarImage = HealthBar.GetComponentInChildren<Image>();
    }

    private void Start()
    {
        onDeath.AddListener(OnDeath);
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
        Health -= damage;
        if(Health < 0)
        {
            Health = 0;
            onDeath.Invoke();
        }
    }

    public void Heal(float healingPoint)
    {
        Health += healingPoint;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    private void OnDeath()
    {
        DestroyImmediate(this.gameObject);
    }

    public float GetHealth()
    {
        return Health;
    }
}
