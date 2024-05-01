using UnityEngine;
using UnityEngine.VFX;

//[RequireComponent(typeof(AudioSource))]
public class GunEffects : MonoBehaviour
{
    [Header("Audios")]
    //[SerializeField] AudioSource gunAudioSource;

    [Header("Effects")]
    [SerializeField] VisualEffect muzzleFlash;


    [SerializeField] float muzzleFlashDuration = 1f;

    private void Awake()
    {
        //gunAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Invoke("DestroyMuzzleflash", muzzleFlashDuration);
    }

    public void SpawnEffect()
    {
        GameObject muzzleflash = BulletPool.Instance.GetMuzzleflash();
        muzzleFlash.Play();
    }

    public void DestroyMuzzleflash()
    {
        BulletPool.Instance.ReturnMuzzleflash(gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
