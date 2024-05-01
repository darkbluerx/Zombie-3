using System;
using UnityEngine;
using TMPro;
using weapon;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set;} // Singleton

    [Header("Drag the text objects here")]
    [Header("Weapon- and ammo text")]

    [SerializeField] TMP_Text weaponNameText;
    [SerializeField] TMP_Text allAmmoText; // Text to display total ammo
    [SerializeField] TMP_Text currentAmmoText; // Text to display current ammo

    [Header("Timer and zombie count text")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text zombieCountText;

    [Header("Images")]
    [SerializeField] Image gunImage; // Image to display current gun
    [SerializeField] Sprite[] sprites; // Array to store all gun images
    [SerializeField] Image staminaBar; // Image to display stamina bar

    [Header("Guns")]
    [SerializeField] Gun[] guns = new Gun[6]; // Array to store all guns (assuming there are 6 guns available)
    Gun currentGun; // The currently selected gun

    private void Awake()
    {
       if(Instance != null)
       {
           Debug.LogWarning("More than one instance of UI found!");
           Destroy(gameObject);
           return;
       }
        Instance = this;
    }

    private void Start()
    {
        SetCurrentGun(0); // Set the default gun (pistol) when the UI starts  
        UpdateAmmoText(); // Update ammo text when the UI starts
    }

    private void OnEnable()
    {
        Gun.OnCurrentAmmoChanged += UpdateAmmoText; // Subscribe to the event to update ammo text when current ammo changes
        Gun.OnAllAmmoChanged += UpdateAmmoText; // Subscribe to the event to update ammo text when total ammo changes
        Gun.OnCurrentGun += SetGunType; // Subscribe to the event to set the current gun when it changes 
        TopDownPlayerController.OnStaminaChanged += UpdateStaminaBar;

        GameInformationWiever.OnTimerText += UpdateTimer; 
        Enemy.OnCountZombieKillEvent += UpdateZombieCount;
    }

    private void UpdateZombieCount()
    {
        if (zombieCountText != null)
        {
            zombieCountText.text = GameInformationWiever.Instance.GetZombieKillCount().ToString();
        }
        //Debug.Log("Zombie count updated: " + GameInformationWiever.Instance.GetZombieKillCount());
    }

    private void UpdateTimer()
    {
        if(timerText != null)
        timerText.text = GameInformationWiever.Instance.elapsedTime.ToString() + " s";
    }

    private void SetCurrentGun(int index) // Set the current gun based on the provided index
    {
        currentGun = guns[index];
  
        SetGunType(currentGun.gunType);
    }

    private void SetGunType(GunType gunType) // Find the gun with the specified type
    {
        currentGun = Array.Find(guns, g => g.gunType == gunType);
        UpdateAmmoText();
        UpdateGunImage();
    }

    public void UpdateAmmoText() // Update total ammo text based on the current gun
    {
        if (allAmmoText != null)
        {
            allAmmoText.text = currentGun.AllAmmo.ToString();
        }

        if (currentAmmoText != null) // Update current ammo text based on the current gun
        {
            currentAmmoText.text = currentGun.CurrentAmmo.ToString() + "/" + currentGun.gunData.magazineSize.ToString();
            //Debug.Log("Ammo updated: " + currentGun.CurrentAmmo);
        }

        if(weaponNameText != null) // Update weapon name text based on the current gun
        {
            weaponNameText.text = currentGun.gunData.gunName;
        }
    }

    private void UpdateGunImage() // Update the gun image based on the current gun
    {
        if (gunImage != null && currentGun != null)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (i == (int)currentGun.gunType)
                {
                    gunImage.sprite = sprites[i];
                }
            }
        }
        else
        {
            Debug.LogError("Gun image or current gun is null");
        }
    }

    private void UpdateStaminaBar()
    {
        staminaBar.fillAmount = TopDownPlayerController.Instance.GetCurrentMaxStamina()/ TopDownPlayerController.Instance.GetMaxStamina();
        //Debug.Log(TopDownPlayerController.Instance.GetCurrentmaxStamina());
    }

    private void OnDisable()
    {
        Gun.OnCurrentAmmoChanged -= UpdateAmmoText; // Unsubscribe from the event when the UI is disabled
        Gun.OnAllAmmoChanged -= UpdateAmmoText; // Unsubscribe from the event when the UI is disabled
        Gun.OnCurrentGun -= SetGunType; // Unsubscribe from the event when the UI is disabled
        TopDownPlayerController.OnStaminaChanged -= UpdateStaminaBar;
        GameInformationWiever.OnTimerText -= UpdateTimer;
    }
}
