using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace weaponUI
{
    public class WeaponData : MonoBehaviour
    {
        [Header("Drag images and texts from the weaponPanel here")]
        public TMP_Text gunKeyNumber;
        public Image gunImage;
        public Image ammoImage; //if no ammo, set active false
    
    }
}
