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
        IsDie = false; // ������ �� IsDie�� false�� �ʱ�ȭ
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
        IsDie = false; // �ٽ� ������ �� IsDie�� false�� ����
    }
}
