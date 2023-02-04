using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private Image HealthBar;

    [SerializeField]
    private float Health, MaxHealth = 100f;
    [SerializeField]
    private float LerpSpeed;

    [SerializeField]
    private bool isEnnemy = false;

    private void Start()
    {
        Health = MaxHealth;
        if (isEnnemy)
            HealthBar.color = Color.red;
    }

    private void Update()
    {
        if(Health > MaxHealth)
            Health = MaxHealth;

        LerpSpeed = 3f * Time.deltaTime;


    }

    public void HealthBarFiller()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, Health / MaxHealth, LerpSpeed);
    }


    public void Damage(float damage)
    {
        Health -= damage;
        if(Health < 0)
        {
            Health = 0;
            //game done
        }
    }

    public void Heal(float healingPoint)
    {
        Health += healingPoint;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

}
