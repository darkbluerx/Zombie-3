using UnityEngine;
using weapon;
using weaponUI; //get weapons data from here

public class WeaponUI : MonoBehaviour
{
    public static WeaponUI Instance { get; private set; } // Singleton

    [Header("Drag images from the weaponPanel here")]
    [SerializeField] GameObject[] weaponUIprefabs; 
    [SerializeField] GameObject[] ammoImages;
    [Space]

    [Header("Drag images and texts from the weaponPanel here")]
    public WeaponUIDataSO[] weaponUIDataSOs;
    [Tooltip("Drag WeaponPanel Guns")] public WeaponData[] weaponDatas;

    public WeaponUIDataSO emptyDatas;

    public bool isEmpty = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is already an instance of WeaponUI in the scene" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetEmptyBoxs(); //set empty data to every weapon panel box
        LoadWeaponData(0); //set pistol data to pistol box (first box)
        AddAmmo(GunType.Pistol, true); //set pistol ammo image active false
        //SetUnactiveWeaponsUIprefabs();
        //AddGun(GunType.Pistol); //set pistol active
    }

    private void OnEnable()
    {
        Item.OnGunAdded += AddGun;
        Gun.OnHaveAmmo += AddAmmo;
    }

    private void SetUnactiveWeaponsUIprefabs()
    {
        for (int i = 0; i < weaponUIprefabs.Length; i++)
        {
            weaponUIprefabs[i].SetActive(false);
        }
    }

    private void LoadWeaponData(int weaponIndex)
    {
        for (int i = 0; i < weaponUIDataSOs.Length; i++)
        {
            if (weaponDatas[i] != null)
            weaponDatas[i].gunKeyNumber.text = weaponUIDataSOs[i].weaponKeyNumber.ToString();

            if (weaponDatas[i] != null)
            weaponDatas[weaponIndex].gunImage.sprite = weaponUIDataSOs[weaponIndex].weaponSprite;
        }
    }

    private void LoadAmmoData(int ammoIndex)
    {
        for (int i = 0; i < weaponUIDataSOs.Length; i++)
        {
            weaponDatas[ammoIndex].ammoImage.sprite = weaponUIDataSOs[ammoIndex].ammoSprite;
        }
    }

    private void AddGun(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.Pistol:
                if(weaponDatas[0] != null)
                weaponUIprefabs[0].SetActive(true);
                LoadWeaponData(0);
                break;
            case GunType.Revolver:
                if (weaponDatas[1] != null)
                    weaponUIprefabs[1].SetActive(true);
                
                LoadWeaponData(1);
                break;
            case GunType.SMG:
                if (weaponDatas[2] != null)
                    weaponUIprefabs[2].SetActive(true);
                LoadWeaponData(2);
                break;
            case GunType.Shotgun:
                if (weaponDatas[3] != null)
                    weaponUIprefabs[3].SetActive(true);
                LoadWeaponData(3);
                break;
            case GunType.Rifle:
                if (weaponDatas[4] != null)
                    weaponUIprefabs[4].SetActive(true);
                LoadWeaponData(4);
                break;
            case GunType.MG:
                if (weaponDatas[5] != null)
                    weaponUIprefabs[5].SetActive(true);
                LoadWeaponData(5);
                break;
            default:
                break;
        }
    }

    private void SetEmptyBoxs()
    {
        for (int i = 0; i < weaponUIprefabs.Length; i++)
        {
            if (isEmpty)
            {
                weaponDatas[i].gunKeyNumber.text = emptyDatas.weaponKeyNumber.ToString();
                weaponDatas[i].ammoImage.sprite = emptyDatas.ammoSprite;
                weaponDatas[i].gunImage.sprite = emptyDatas.weaponSprite;
            }
        }   
    }

    public void AddAmmo(GunType gunType, bool haveAmmo) //set ammo image active true or false
    {
        switch (gunType)
        {
            case GunType.Pistol:
                ammoImages[0].SetActive(haveAmmo);
                LoadAmmoData(0);
                break;
            case GunType.Revolver:
                ammoImages[1].SetActive(haveAmmo);
                LoadAmmoData(1);
                break;
            case GunType.SMG:
                ammoImages[2].SetActive(haveAmmo);
                LoadAmmoData(2);
                break;
            case GunType.Shotgun:
                ammoImages[3].SetActive(haveAmmo);
                LoadAmmoData(3);
                break;
            case GunType.Rifle:
                ammoImages[4].SetActive(haveAmmo);
                LoadAmmoData(4);
                break;
            case GunType.MG:
                ammoImages[5].SetActive(haveAmmo);
                LoadAmmoData(5);
                break;
            default:
                break;
        }
    }
}
