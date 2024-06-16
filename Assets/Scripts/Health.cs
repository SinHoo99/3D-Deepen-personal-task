using System;
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
        IsDie = false; // 시작할 때 IsDie를 false로 초기화
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

    public void Respawn()
    { 
        maxHealth = Mathf.RoundToInt(maxHealth * 1.1f);
        health = maxHealth;
        IsDie = false; // 다시 생성될 때 IsDie를 false로 설정
    }
}
