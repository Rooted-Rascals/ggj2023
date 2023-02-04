using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    UnityEvent _ObjectDeath;


    GameObject HealthBar;

    private void Start()
    {
        Health = MaxHealth;
        if (isEnnemy)
            HealthBarImage.color = Color.red;
        HealthBar = Instantiate(Resources.Load("Prefab/HealthBar"), new Vector3(0,Height,0), Quaternion.identity) as GameObject;
        HealthBar.transform.parent = gameObject.transform;
        HealthBarImage = HealthBar.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if(Health > MaxHealth)
            Health = MaxHealth;

        LerpSpeed = 3f * Time.deltaTime;


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
