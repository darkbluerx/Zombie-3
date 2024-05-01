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
    [RequireComponent(typeof(AudioSource))]
    public class Gun : Weapon
    {
        public static Action<bool> OnGetGun; //play gun movement animations

        public static event Action OnAllAmmoChanged;
        public static event Action OnCurrentAmmoChanged;
        public static event Action<GunType> OnCurrentGun;
        public static event Action<GunType, bool> OnHaveAmmo;
        public static event Action OnShootAnimation; //play the shoot animation
        public static event Action OnGetDamageAmount;

        public GunType gunType; //the type of the gun

        [SerializeField] string gunName = "Gun";

        [SerializeField] AudioSource audioSource;
        [Space]

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
            audioSource = GetComponent<AudioSource>();
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
        }


        // Muutetaan haveGun22 arrayksi
        public bool[] haveGun22 = new bool[Enum.GetValues(typeof(GunType)).Length];

        private void HaveThisGun(bool haveGun, GunType gunType)
        {
            // Asetetaan oikea boolean arvo oikeaan indeksiin
            int gunIndex = (int)gunType;
            haveGun22[gunIndex] = haveGun;

            // Tarkistetaan, onko jokin muu ase kerätty, jos ei, kutsutaan OnGetGun
            bool anyGunCollected = Array.Exists(haveGun22, gun => gun);
            OnGetGun?.Invoke(anyGunCollected);
        }

        // Lisää bool-muuttuja, joka kertoo, onko tämä ase valittuna
        private bool isSelected;

        // Metodi asettaa tämän aseen valituksi
        public void SetSelected(bool selected)
        {
            isSelected = selected;
        }

        private void Update()
        {
            ShootInput();
            ReloadInput();

            // Kutsutaan HaveThisGun-metodia vain, jos tämä ase on valittuna
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
                gunData.shoot.Play(audioSource);
                OnShootAnimation?.Invoke();

                for (int i = 0; i < gunData.bulletsPerShot; i++)
                {
                    // Orginal versio

                    //GameObject bullet = BulletPool.Instance.GetBullet();
                    //bullet.transform.position = firePoint.position;
                    //bullet.transform.rotation = firePoint.rotation;
                    //ProjectileScatter();

                    //Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                    //bulletRb.velocity = transform.forward * gunData.bulletSpeed + bulletSpread;
                    //AllAmmo--;


                    //SpawnEffect();
                    //OnSpawnEffect?.Invoke();
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


                    //Find mouse position in the world
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //RaycastHit hit;

                    //int layerMask = LayerMask.GetMask("Ground");

                    //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                    //{
                    //    //Aim the bullet towards the mouse target horizontally
                    //    Vector3 targetPosition = new Vector3(hit.point.x, firePoint.position.y, hit.point.z - 1.8f);
                    //    bullet.transform.LookAt(targetPosition);
                    //    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * gunData.bulletSpeed + ProjectileScatter();
                    //}         
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

        //commented when not needed
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawRay(firePoint.position, bulletSpread * 10);
        //}

        public override IEnumerator Reload()
        {
            gunData.reload.Play(audioSource);
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