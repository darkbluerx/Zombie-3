using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    //Events
    public event Action<int> OnTakeDamage; 
    public event Action OnDeadEvent; //play dead animation -> ZombieAnimations.cs
    public event Action OnEat; //play eat animation -> ZombieAnimations.cs

    [Header("Drag unit stats")]
    [SerializeField] UnitsStatsSO unitsStats;

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
            OnTakeDamage?.Invoke(amount); // Call the OnTakeDamage event and pass the amount of damage

            //Check if the character is dead
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeadEvent?.Invoke(); //play dead animation -> ZombieAnimations.cs
                OnEat?.Invoke(); //play eat animation -> ZombieAnimations.cs
            }
        }
        return CurrentHealth;
    }
}