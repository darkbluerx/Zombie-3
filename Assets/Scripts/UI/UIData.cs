using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData : MonoBehaviour
{
    //public event Action<int> OnAmmoChanged;
    public event Action<int> OnHealthChanged;

    [SerializeField] int ammo;
    [SerializeField] string weaponName;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    //public int Ammo
    //{
    //    get => ammo;
    //    set
    //    {
    //        ammo = value;
    //        OnAmmoChanged?.Invoke(ammo);
    //    }
    //}

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
