using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int health;
    public event Action OnDie;
    public bool IsDie = false;

    public bool IsDead => health == 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            OnDie?.Invoke();
            IsDie = true;
        }

        Debug.Log(health);
    }
}
