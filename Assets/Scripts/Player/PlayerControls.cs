using UnityEngine;
using System;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; } //Singleton

    //change player health
    [SerializeField] UnitsStats unitsStats;

    [SerializeField] float health;

    public event Action<float> OnHealthChanged;
    //public event Action OnMaxHealthChanged;

    public float Health
    {
        get => health;
        set
        {
            health = value;

            OnHealthChanged?.Invoke(health);
        }
    }

    private void Awake()
    {
        unitsStats = Resources.Load<UnitsStats>("ScriptableObjects/Player/UnitsStats");
       
        if(Instance != null)
        {
            Debug.LogWarning("There is already an instance of PlayerControls in the scene" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        health = unitsStats.maxHealth;
    }
}

