using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName ="Items/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] string itemName;
    public AudioEvent pickUpAudioEvent;
    public GameObject itemPrefab;
    public int minAmount;
    public int maxAmount;
}
