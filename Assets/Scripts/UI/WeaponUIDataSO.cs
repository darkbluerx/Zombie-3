using UnityEngine;
using weaponUI;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponUIDataSO", menuName = "weaponUIData/WeaponData")]
public class WeaponUIDataSO : ScriptableObject
{
    [Header("Weapon Image")]
    public Sprite weaponSprite;
    [Space]

    [Header("Ammo Image")]
    public Sprite ammoSprite;
    [Space]

    [Header("Weapon KeyNumber")]
    public int weaponKeyNumber;
}
