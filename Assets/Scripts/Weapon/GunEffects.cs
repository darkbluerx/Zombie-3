using UnityEngine;
using UnityEngine.VFX;

//This is where muzzleflash playback is performed
public class GunEffects : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] VisualEffect muzzleFlash;

    [SerializeField] float muzzleFlashDuration = 1f;

    private void OnEnable()
    {
        Invoke("DestroyMuzzleflash", muzzleFlashDuration);
    }

    public void SpawnEffect()
    {
        GameObject muzzleflash = BulletPool.Instance.GetMuzzleflash(); //Get the muzzleflash from the pool
        muzzleFlash.Play();
    }

    public void DestroyMuzzleflash()
    {
        BulletPool.Instance.ReturnMuzzleflash(gameObject); //Return the muzzleflash to the pool
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
