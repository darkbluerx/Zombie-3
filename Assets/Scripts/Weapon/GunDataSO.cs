using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData", order = 1)]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public string gunName;
    [Multiline(10)] string description;
    public Sprite itemSprite;
    //public GameObject muzzleEffectPrefab;
    //public GameObject line;
    [Space]

    [Header("Audios")]
    public AudioEvent shoot;
    public AudioEvent reload;
    //hit
    //empty
    [Space]

    [Header("Gun Attributes")]
    [Range(10f, 1000f)] public float fireRate = 100f;
    [Range(1, 200)] public int damage = 10;
    [Range(1, 100)] public int magazineSize = 30;
    [Range(0.5f, 10f)] public float reloadTime = 1f;
    [Space]
  
    [Header("Bullet parameters")]
    [Range(0f, 10f)] public float scatterAngle = 1f; // Angle of scatter in degrees
    [Range(1, 100)] public int bulletsPerShot = 1; // Number of bullets to fire in one shot
    [Range(0.5f, 100f)] public float bulletSpeed = 10f;
    [Range(0.5f, 10f)] public float bulletLifeTime = 2f; //range of bullet life time
    [Space]

    [Header("Physical Forces")]
    [Range(0.1f, 10f)] public float pushingForce = 1f; // push enemy back
    [Range(0.1f, 10f)] public float slowMotion = 1f; // slow enemy movement
    // You can add more properties as needed

    // This method can be used to play the shoot sound
    //public void PlayShootSound(AudioSource audioSource)
    //{
    //    audioSource.PlayOneShot(shootSound);
    //}
}