using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitsStatsSO : ScriptableObject
{
    public string unitName;
    [Multiline(10)] string description;
    //public Sprite unitSprite;
    //public GameObject muzzleEffectPrefab;
    [Space]

    [Header("Audios")]
    public AudioEvent hitAudioEvent;
    public AudioEvent deathAudioEvent;
    public AudioEvent shoutAudioEvent;
    //hit
    //shouts
    //death
    [Space]

    [Header("Unit Attributes")]
    [Range(0.1f, 100f)] public float moveSpeed = 0.5f;
    [Range(0.1f, 100f)] public float runSpeed = 0.5f;
    [Range(0.1f, 100f)] public float stamina = 0.5f;
    [Range(1, 100f)] public int minHealth = 20;
    [Range(1, 200f)] public int maxHealth = 100;

}
