using System;
using UnityEngine;
using weapon;


public class ItemManager : MonoBehaviour
{
    [Header("Add all weapon classes manually")]

    // Käytä [SerializeField] ja anna kullekin Gun-elementille nimi Unity Editorissa
    [SerializeField] Gun[] guns;
    [Space]

    [SerializeField] GameObject[] gunModels;

    //[Header("Show you ammos")]
    public int[] ammoList;

    private void Awake()
    {
        ammoList = new int[6];
    }

    private void OnEnable()
    {
        Item.OnItemAdded += AddAmmo;   
    }

    private void AddAmmo(int amount, ItemType itemType, GunType gunType)
    {
        Gun gun = Array.Find(guns, g => g.gunType == gunType);
        if (gun != null)
        {
            gun.allAmmo += amount;
            
            ammoList[(int)gunType] = gun.allAmmo;
            //Debug.Log($"{gunType} ammo added. Max Ammo: {gun.maxAmmo}");
        }
        Gun.OnAllAmmoChanged += UI.Instance.UpdateAmmoText; // Subscribe to the event to update ammo text when total ammo changes
    }

    private void OnDisable()
    {
        Item.OnItemAdded -= AddAmmo;
    }
}