using UnityEngine;

public class PlayerHint : MonoBehaviour
{
    public GuidanceSO hint;
    IHintPrinterable hintPrinter;

    private void OnEnable()
    {
        hintPrinter = FindObjectOfType<HintPrinter>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintPrinter != null)
            {
                hintPrinter.PrintHintText(hint); //print the hint
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintPrinter != null)
            {
                hintPrinter.PrintHintText(null); //print nothing
            }
        }
    }

    public void ResetHintText()
    {
        hintPrinter.PrintHintText(null); //print nothing
    }
}
