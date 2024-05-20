using UnityEngine;
using weapon;
using System;

public enum ItemType
{
    PistolAmmo,
    RevolverAmmo,
    SMGAmmo,
    ShotgunAmmo,
    RifleAmmo,
    MGAmmo,
}

[System.Serializable]
public class Item : MonoBehaviour
{
    //public AudioSource audioSource;

    public static event Action<int, ItemType, GunType> OnItemAdded;
    public static event Action<GunType> OnGunAdded; //get gun model
    public static event Action<bool, GunType> OnHaveThisGun;
    //public static event Action<bool> OnHaveThisGunTest;

    [Header("Drag the item information here")]
    [SerializeField] ItemSO itemData;

    [Header("Choose ammo type")]
    public ItemType itemType = ItemType.PistolAmmo;
    [Space]

    [Header("Choose weapon type")]
    public GunType gunType;
    [Space]

    [Header("Is this ammo/weapon?")]
    [SerializeField] bool isAmmo = true;

    private void Awake()
    {
        //audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAmmo) //collect ammo and amount
        {
            //itemData.pickUpAudioEvent.Play(audioSource);
            AudioManager.Instance.PlayCorrectSound(audioEvent: itemData.pickUpAudioEvent);
            var randomAmount = UnityEngine.Random.Range(itemData.minAmount, itemData.maxAmount);

            CollectItem(randomAmount);
        }
        else if (other.CompareTag("Player") && !isAmmo) //collect weapon
        {
            //itemData.pickUpAudioEvent.Play(audioSource);
            AudioManager.Instance.PlayCorrectSound(audioEvent: itemData.pickUpAudioEvent);
            CollectWeapon(gunType);
        }

        Destroy(gameObject);
    }

    public void CollectItem(int amount)
    {
        //itemData.pickUpAudioEvent.Play(audioSource);
        AudioManager.Instance.PlayCorrectSound(audioEvent: itemData.pickUpAudioEvent);
        OnItemAdded?.Invoke(amount, itemType, gunType); //Invoke the event
        WeaponUI.Instance.AddAmmo(gunType, true); //set ammo image active true
        UI.Instance.UpdateAmmoText(); //update ammo text
    }

    public void CollectWeapon(GunType gunType)
    {
        OnGunAdded?.Invoke(gunType);  //call event in WeaponUI script AddGun(gunType), set gun UI active
        OnHaveThisGun?.Invoke(true, gunType); //
        //OnHaveThisGunTest?.Invoke(true); //
    }
}
