using UnityEngine;

//Scriptable object for the guidance text
[CreateAssetMenu(fileName = "Guidance", menuName ="UI/Text")]
public class GuidanceSO : ScriptableObject
{
    public int level;
    [Multiline ()] public string  guindanceText;   
}
