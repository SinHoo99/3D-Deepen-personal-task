using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;

    private List<Collider> alreadyColliderWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyColliderWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyColliderWith.Contains(other)) return;

        alreadyColliderWith.Add(other);

        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;
        if (damage < 0)
        {
            Debug.LogError("Weapon: Damage cannot be negative.");
            return;
        }
    }
}