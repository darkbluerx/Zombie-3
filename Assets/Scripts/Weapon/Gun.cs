using System;
using System.Collections;
using UnityEngine;

namespace weapon
{
    public enum GunType
    {
        Pistol,
        Revolver,
        SMG, //SubmachineGun
        Shotgun,
        Rifle,
        MG, //MachineGun
    }

    [System.Serializable]
    //[RequireComponent(typeof(AudioSource))]
    public class Gun : Weapon
    {
        Gun gunComponent;

        public static Action<bool> OnGetGun; //play gun movement animations

        public static event Action OnAllAmmoChanged; //UI Manager update ammo text
        public static event Action OnCurrentAmmoChanged; //UI Manager update ammo text
        public static event Action<GunType> OnCurrentGun; 
        public static event Action<GunType, bool> OnHaveAmmo; //UI Manager update ammo text
        public static event Action OnShootAnimation; //play the shoot animation

        public GunType gunType; //the type of the gun

        //[SerializeField] AudioSource audioSource;
        //[Space]

        [Header("Gun Data and features")]
        public GunData gunData;

        [Header("Choose your Inputs")]
        [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
        [SerializeField] KeyCode reloadKey = KeyCode.R;
        [Space]

        [SerializeField] Transform muzzleflashPosition; //the position of the muzzleflash effect

        public override Vector3 bulletSpread { get; set; }
        [field: SerializeField] public override GameObject bulletPrefab { get; set; }
        [field: SerializeField] public override Transform firePoint { get; set; } // Where the bullet is spawned

        //Bools
        public override bool canShoot { get; set; }
        public override bool isReloading { get; set; }
        public bool[] haveGun22 = new bool[Enum.GetValues(typeof(GunType)).Length]; //Collect all the guns
        private bool isSelected; //Add a bool variable that tells if this gun is selected

        float timeSinceLastShot;

        public GunType GetGunType()
        {
            return gunType;
        }

        public int allAmmo = 10; //the amount of bullets what you have
        public int AllAmmo //update the all ammo
        {
            get => allAmmo;
            set
            {
                allAmmo = value;
                OnAllAmmoChanged?.Invoke(); //UI Manager update ammo text

                if (allAmmo == 0)
                {
                    WeaponUI.Instance.AddAmmo(gunType, false); //set ammo image active false
                }        
            }
        }

        public override int currentAmmo { get; set; } //the amount of bullets in the magazine  
        public int CurrentAmmo //update the current ammo
        {
            get => currentAmmo;
            set
            {
                currentAmmo = value;
                OnCurrentAmmoChanged?.Invoke();             
               // Debug.Log($"Current ammo: {currentAmmo}");
            }
        }

        private void Awake()
        {
            gunType = GetGunType();
            //audioSource = GetComponent<AudioSource>();
            currentAmmo = gunData.magazineSize; //the amount of bullets in the magazine  
        }

        private void Start()
        {
            OnGetGun?.Invoke(true);
        }

        private void OnEnable()
        {
            if (allAmmo <= gunData.magazineSize)
            {
                currentAmmo = allAmmo;
            }
            OnCurrentGun?.Invoke(gunType); // Invoke the event to set the current gun, update ammo text when switching guns

            Item.OnHaveThisGun += HaveThisGun;
            Settings.OnDisableGun += DisableGun;
            Settings.OnEnableGun += EnableGun;
        }

        public void DisableGun()
        {
            if(gunComponent != null)
            {
                gunComponent.enabled = false;
            }
        }

        public void EnableGun()
        {
            if (gunComponent != null)
            {
                gunComponent.enabled = true;
            }
        }
   
        private void HaveThisGun(bool haveGun, GunType gunType)
        {       
            //Set the correct boolean value to the correct index
            int gunIndex = (int)gunType;
            haveGun22[gunIndex] = haveGun;

            //Check if any gun is collected, if not, call OnGetGun
            bool anyGunCollected = Array.Exists(haveGun22, gun => gun);
            OnGetGun?.Invoke(anyGunCollected);
        }

        private void Update()
        {
            ShootInput();
            ReloadInput();

            // Call the HaveThisGun method only if this gun is selected
            if (isSelected)
            {
                HaveThisGun(haveGun22[(int)gunType], gunType);
            }
        }

        private void ReloadInput()
        {
            if (Input.GetKeyDown(reloadKey)) StartReload();
        }

        private void ShootInput()
        {
            if (Input.GetKeyDown(shootKey) || (Input.GetKey(shootKey))) Shoot();
        }

        //Method sets this gun as selected
        public void SetSelected(bool selected)
        {
            isSelected = selected;
        }

        private void FixedUpdate()
        {
            timeSinceLastShot += Time.deltaTime;      
        }

        private void StartReload()
        {
            if (!isReloading && allAmmo> 0 && this.gameObject.activeSelf) StartCoroutine(Reload());  
        }

        private bool CanShoot() => !isReloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

        private void Shoot()
        {
            if (currentAmmo <= 0 || allAmmo <= 0) return;
            
            if(CanShoot())
            {
                //gunData.shoot.Play(audioSource);
                AudioManager.Instance.PlayCorrectSound(audioEvent: gunData.shoot);
                OnShootAnimation?.Invoke(); //play the shoot animation

                for (int i = 0; i < gunData.bulletsPerShot; i++)
                {
                    GameObject muzzleflash = BulletPool.Instance.GetMuzzleflash();
                    muzzleflash.transform.position = muzzleflashPosition.position;
                    muzzleflash.transform.rotation = muzzleflashPosition.rotation;
                    
                    // New Version
                    GameObject bulletObject = BulletPool.Instance.GetBullet();
                    //bulletObject.transform.position = firePoint.position;
                    //bulletObject.transform.rotation = firePoint.rotation;

                    var barrelTransform = TopDownPlayerController.Instance._firePoint;
                    var barrelRotation = TopDownPlayerController.Instance._firePoint.rotation;

                    TopDownPlayerController.Instance._firePoint = firePoint;
                    TopDownPlayerController.Instance._firePoint.rotation = firePoint.rotation;

                    bulletObject.transform.position = barrelTransform.position;
                    bulletObject.transform.rotation = barrelTransform.rotation;
                  
                    Bullet bullet = bulletObject.GetComponent<Bullet>();
                    if (bullet != null)
                    {
                        bullet.SetDamage(gunData.damage); // Set the damage amount for the bullet
                    }

                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * gunData.bulletSpeed + ProjectileScatter();
       
                }
                AllAmmo--; // Reduce the all ammo by one
                CurrentAmmo--; // Reduce the current ammo by one

                timeSinceLastShot = 0f;
            }
        }

        public Vector3 ProjectileScatter()
        {
            float randomSpreadX = UnityEngine.Random.Range(-gunData.scatterAngle, gunData.scatterAngle);
            float randomSpreadZ = UnityEngine.Random.Range(-gunData.scatterAngle, gunData.scatterAngle);

            return bulletSpread = new Vector3(randomSpreadX, 0, randomSpreadZ);
        }

        //commented when not needed, draw a ray to see the bullet spread
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawRay(firePoint.position, bulletSpread * 10);
        //}

        public override IEnumerator Reload()
        {
            //gunData.reload.Play(audioSource);
            AudioManager.Instance.PlayCorrectSound(audioEvent: gunData.reload);
            isReloading = true;
           
            yield return new WaitForSeconds(gunData.reloadTime);

            if (allAmmo >= gunData.magazineSize)
            {
                currentAmmo = gunData.magazineSize;
                allAmmo -= gunData.magazineSize-currentAmmo;
            }
            if(allAmmo <= gunData.magazineSize)
            {
                currentAmmo = allAmmo;
            }

            OnAllAmmoChanged?.Invoke();
            isReloading = false;
        }

        private void OnDisable()
        {
            //PlayerShoot.Instance.OnReloadInput -= StartReload;
            //PlayerShoot.Instance.OnShootInput -= Shoot;
            isReloading = false;
        }
    }
}