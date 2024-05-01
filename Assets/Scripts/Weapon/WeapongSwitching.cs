//using System;
//using UnityEngine;
//using weapon;

//public class WeapongSwitching : MonoBehaviour
//{
    //public static Action<bool> OnGetGun; //play gun movement animations

    //// References to different weapons and their models
    //[Header("References")]
    //[SerializeField] Transform[] weapons; // Array of weapon transforms (empty game objects to position weapons)
    //[SerializeField] GameObject[] gunModels; // Array of gun models (actual visual representations of guns)
    //[Space]

    //// Keys used to switch between weapons
    //[Header("Keys")]
    //[SerializeField] KeyCode[] keys;

    //// Settings
    //[Header("Settings")]
    //[SerializeField] float switchTime; // Time between switching weapons

    //int selectedWeapon; // Index of the currently selected weapon
    //float timeSinceLastSwitch; // Time passed since the last weapon switch

    //bool hasGun = false;

    //void Start()
    //{
    //    //AddGun(GunType.Pistol);
    //    // Initialize the weapon setup and select the initial weapon

    //    bools[0] = true;
    //    SetWeapons();
    //    Select(selectedWeapon);

    //    // Initialize the timeSinceLastSwitch variable
    //    timeSinceLastSwitch = 0f;
    //}

    //private void OnEnable()
    //{
    //    // Subscribe to the GunAdded event when this component is enabled
    //    Item.OnGunAdded += FindGun;
    //}

    //private void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        OnGetGun?.Invoke(true);

    //        Debug.Log("T pressed");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        OnGetGun?.Invoke(false);

    //        Debug.Log("Y pressed");
    //    }

    //    // Store the previous selected weapon index
    //    int previousSelectedWeapon = selectedWeapon;

    //    // Check for key presses to switch weapons
    //    for (int i = 0; i < keys.Length; i++)
    //    {
    //        if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
    //        {
    //            selectedWeapon = i;
    //            //Select(selectedWeapon);
    //            //if (selectedWeapon == 0) OnGetGun?.Invoke(true); //play gun movement animations

    //        }
    //    }

    //    // If the selected weapon has changed, call the Select method
    //    if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);

    //    // Update the timeSinceLastSwitch
    //    timeSinceLastSwitch += Time.deltaTime;
    //}

  

 
    //// Set up the weapons array based on the child transforms of the current GameObject
    //private void SetWeapons()
    //{
    //    weapons = new Transform[transform.childCount];

    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        weapons[i] = transform.GetChild(i);
    //    }

    //    // If keys are not initialized, create an array with the same length as the weapons array
    //    if (keys == null) keys = new KeyCode[weapons.Length];
    //}

    //private void Select(int weaponIndex)
    //{
    //    for (int i = 0; i < weapons.Length; i++)
    //    {
    //        // Activate the selected weapon and deactivate others
    //        bool isSelected = (i == weaponIndex);
    //        weapons[i].gameObject.SetActive(isSelected);

    //        //bools[i] = weaponIndex == i;

    //        for (int le = 0; le < bools.Length; le++)
    //        {

    //            if (isSelected && bools[0] == true)
    //            {
    //                OnGetGun?.Invoke(true);
    //            }
    //            if (isSelected && bools[le] == false)
    //            {
    //                OnGetGun?.Invoke(false);
    //            }
    //        }
    //    }

    //    // Reset the timeSinceLastSwitch to 0 when a new weapon is selected
    //    timeSinceLastSwitch = 0f;
    //}

    //private GunType GetGunTypeForIndex(int index)
    //{
    //    switch (index)
    //    {
    //        case 0:
    //            return GunType.Pistol;
    //        case 1:
    //            return GunType.Revolver;
    //        case 2:
    //            return GunType.SMG;
    //        case 3:
    //            return GunType.Shotgun;
    //        case 4:
    //            return GunType.Rifle;
    //        case 5:
    //            return GunType.MG;
    //        default:
    //            return GunType.Pistol;
    //    }
    //}

    //private void OnDisable()
    //{
    //   // Item.OnGunAdded -= AddGun;
    //}


using System;
using UnityEngine;
using weapon;

public class WeaponSwitching : MonoBehaviour
{
    public static Action<bool> OnGetGun; // play gun movement animations

    // References to different weapons and their models
    [Header("References")]
    [SerializeField] Transform[] weapons; // Array of weapon transforms (empty game objects to position weapons)
    [SerializeField] GameObject[] gunModels; // Array of gun models (actual visual representations of guns)
    [Space]

    // Keys used to switch between weapons
    [Header("Keys")]
    [SerializeField] KeyCode[] keys;

    // Settings
    [Header("Settings")]
    [SerializeField] float switchTime; // Time between switching weapons

    int selectedWeapon; // Index of the currently selected weapon
    float timeSinceLastSwitch; // Time passed since the last weapon switch

    bool[] bools = new bool[6]; // Array to track if each weapon is acquired

    void Start()
    {
        // Initialize the weapon setup and select the initial weapon
        bools[0] = true; // Assuming the first weapon is initially available
        SetWeapons();
        Select(selectedWeapon);

        // Initialize the timeSinceLastSwitch variable
        timeSinceLastSwitch = 0f;
    }

    private void OnEnable()
    {
        // Subscribe to the GunAdded event when this component is enabled
        Item.OnGunAdded += FindGun;  
    }

    private void Update()
    {
        // Store the previous selected weapon index
        int previousSelectedWeapon = selectedWeapon;

        // Check for key presses to switch weapons
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
            }
        }

        // If the selected weapon has changed, call the Select method
        if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);

        // Update the timeSinceLastSwitch
        timeSinceLastSwitch += Time.deltaTime;
    }

    private void FindGun(GunType type)
    {
        // Activate the gun model corresponding to the acquired gun
        int gunIndex = (int)type;
        if (gunIndex >= 0 && gunIndex <= gunModels.Length)
        {
            gunModels[gunIndex].SetActive(true);
            bools[gunIndex] = true;
        }
    }

    // Set up the weapons array based on the child transforms of the current GameObject
    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        // If keys are not initialized, create an array with the same length as the weapons array
        if (keys == null) keys = new KeyCode[weapons.Length];
    }

    private void Select(int weaponIndex)
    {
        bool isAvailable = bools[weaponIndex];

        // If the selected weapon is available, select it and invoke OnGetGun with true
        if (isAvailable)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                bool isSelected = (i == weaponIndex);
                weapons[i].gameObject.SetActive(isSelected);
            }

            OnGetGun?.Invoke(true);
        }
        else
        {
            // If none of the weapons are available, invoke OnGetGun with false
            bool anyAvailable = false;
            foreach (bool available in bools)
            {
                if (available)
                {
                    anyAvailable = true;
                    break;
                }
            }

            OnGetGun?.Invoke(anyAvailable);
        }

        // Reset the timeSinceLastSwitch to 0 when a new weapon is selected
        timeSinceLastSwitch = 0f;
    }


    private void OnDisable()
    {
        // Unsubscribe from the GunAdded event when this component is disabled
        Item.OnGunAdded -= FindGun;
    }
}


