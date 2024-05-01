using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveData", menuName = "ScriptableObjects/ExplosiveData", order = 1)]
public class ExplosiveDataSO : ScriptableObject
{
    public string gunName;
    [Multiline(10)] string description;
    //public Sprite gunSprite;
    //public GameObject muzzleEffectPrefab;
    //public GameObject explosionEffectPrefab;
    //public GameObject line;
    [Space]

    [Header("Audios")]
    public AudioEvent shoot;
    public AudioEvent reload;
    public AudioEvent hit; //explosion
    //empty
    [Space]

    [Header("Explosive Attributes")]
    [Range(0.1f, 100f)] public float fireRate = 0.5f;
    [Range(5f, 200f)] public float projectileSpeed = 10f;
    [Range(1, 200)] public float damage = 10;
    [Range(10, 100)] public int magazineSize = 30;
    [Range(0.5f, 10f)] public float reloadTime = 1f;
    [Range(0.5f, 10f)] public float bulletSpeed = 10f;
    [Range(0.5f, 10f)] public float bulletLifeTime = 2f;

    [Range(0.5f, 10f)] public float explosinRadius = 2f;


    [Header("Physical Forces")]
    [Range(0.5f, 10f)] public float knockbackForce = 1f;
    [Range(0.5f, 10f)] public float stoppingPower = 1f;
    // You can add more properties as needed

    // This method can be used to play the shoot sound
    //public void PlayShootSound(AudioSource audioSource)
    //{
    //    audioSource.PlayOneShot(shootSound);
    //}
}