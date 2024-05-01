using UnityEngine;
using UnityEngine.UI;
using weapon;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Image weaponImage;

    readonly Gun currentGun;
    GunData[] availableGuns;

    // Luodaan eventit aseen vaihdolle ja ammusm‰‰r‰n p‰ivitykselle
    public delegate void GunChangedHandler(Gun newGun);
    public static event GunChangedHandler OnGunChanged;

    public delegate void AmmoChangedHandler(int newAmmo);
    public static event AmmoChangedHandler OnAmmoChanged;

    void Start()
    {
        // Oletetaan, ett‰ Gun-luokka ja ScriptableGun-luokka on jo toteutettu.

        // Alusta saatavilla olevat aseet
        //availableGuns = Resources.LoadAll<ScriptableGun>("ScriptableGuns");

        // Alusta k‰ytˆss‰ oleva ase
        SetCurrentGun(0);
    }

    private void SetCurrentGun(int index)
    {
        if (index >= 0 && index < availableGuns.Length)
        {
            //currentGun = new Gun(availableGuns[index]);

            // L‰het‰ tapahtuma, ett‰ ase on vaihtunut
            OnGunChanged?.Invoke(currentGun);

            // P‰ivit‰ ammusm‰‰r‰teksti
            UpdateAmmoText();

            // P‰ivit‰ aseen kuva
            UpdateWeaponImage();
        }
        else
        {
            Debug.LogError("Invalid gun index.");
        }
    }

    public void UpdateAmmoText()
    {
        // P‰ivit‰ ammusm‰‰r‰teksti
        if (ammoText != null)
        {
            //ammoText.text = "Ammo: " + currentGun.Ammo.ToString();

            // L‰het‰ tapahtuma, ett‰ ammusm‰‰r‰ on p‰ivittynyt
            //OnAmmoChanged?.Invoke(currentGun.Ammo);
        }
    }
     
    private void UpdateWeaponImage()
    {
        // P‰ivit‰ aseen kuva
        if (weaponImage != null)
        {
            //weaponImage.sprite = currentGun.GunSprite;
        }
    }

    // Voit kutsua t‰t‰ metodia jostain muualta peliss‰, esimerkiksi aseen vaihdon yhteydess‰
    public void ChangeWeapon(int index)
    {
        SetCurrentGun(index);
    }
}
