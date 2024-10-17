using System.Collections.Generic;
using UnityEngine;

/*
 * Here object bool is used for firing projectiles. With this we adjust better performance.
 * The idea of object boolean is to reuse the same objects instead of creating new ones.
 */
public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; } //Singleton

    [SerializeField] GameObject bulletPrefab;
    [SerializeField, Tooltip("Fired projectiles object bool size")] int poolSize = 30;
    [Space]

    [SerializeField] GameObject muzzleflashPrefab;
    [SerializeField, Tooltip("Muzzleflash object bool size")] int muzzleflashPoolSize = 30;

    List<GameObject> bulletsBool; //List of fired projectiles
    List<GameObject> muzzleflashes; //List of muzzleflashes

    private void Awake()
    {
        if (Instance != null) //Singleton
        {
            Debug.LogError("There's more than one BulletBool! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        bulletsBool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletsBool.Add(bullet);
        }

        muzzleflashes = new List<GameObject>();

        for (int i = 0; i < muzzleflashPoolSize; i++)
        {
            GameObject muzzleflash = Instantiate(muzzleflashPrefab);
            muzzleflash.SetActive(false);
            muzzleflashes.Add(muzzleflash);
        }
    }

    public GameObject GetBullet() //Get the fired projectile
    {
        foreach (var bullet in bulletsBool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        bulletsBool.Add(newBullet);
        return newBullet;
    }


    public GameObject GetMuzzleflash() //Get the muzzleflash
    {
        foreach (var muzzleflash in muzzleflashes)
        {
            if (!muzzleflash.activeInHierarchy)
            {
                muzzleflash.SetActive(true);
                return muzzleflash;
            }
        }

        GameObject newMuzzleflash = Instantiate(muzzleflashPrefab);
        muzzleflashes.Add(newMuzzleflash);
        return newMuzzleflash;
    }

    public void ReturnBullet(GameObject bullet) //Return the fired projectile
    {
        bullet.SetActive(false);
    }

    public void ReturnMuzzleflash(GameObject muzzleflash)
    {
        muzzleflash.SetActive(false);
    }
}
