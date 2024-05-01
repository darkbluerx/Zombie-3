using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public event Action<int> OnTakeDamage;
    public event Action OnDeadEvent;
   
    [SerializeField] UnitsStats unitsStats;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    private void Start()
    {
        int randomHealth = UnityEngine.Random.Range(unitsStats.minHealth, unitsStats.maxHealth);

        MaxHealth = randomHealth;
        CurrentHealth = MaxHealth;
        //Debug.Log($"Max health: {MaxHealth}");
    }

    public int TakeDamage(int amount)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= amount;

            // Call the OnTakeDamage event and pass the amount of damage
            OnTakeDamage?.Invoke(amount); 

            //Check if the character is dead
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeadEvent?.Invoke();
            }
        }
        return CurrentHealth;
    }
}