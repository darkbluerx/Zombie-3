using UnityEngine;
using TMPro;

public class HintPrinter : MonoBehaviour, IHintPrinterable
{
    public TMP_Text hintText;

    public void PrintHintText(GuidanceSO hint)
    {
        if (hintText != null)
        {
            hintText.text = hint != null ? hint.guindanceText : ""; //if hint is not null, print the hint, else print nothing
        }
    }
}
