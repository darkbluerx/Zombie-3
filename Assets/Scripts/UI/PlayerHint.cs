using UnityEngine;

//Giving tips to the player
public class PlayerHint : MonoBehaviour
{
    public GuidanceSO hint; //The hint to be shown, scriptable object
    IHintPrinterable hintPrinter; //The hint printer interface

    private void OnEnable()
    {
        hintPrinter = FindObjectOfType<HintPrinter>();
    }

    private void OnTriggerStay(Collider other) // Show the hint when the player is near
    {
        if (other.CompareTag("Player"))
        {
            if (hintPrinter != null)
            {
                hintPrinter.PrintHintText(hint); //Print the hint
            }
        }
    }

    private void OnTriggerExit(Collider other) //Disable the hint when the player leaves
    {
        if (other.CompareTag("Player"))
        {
            if (hintPrinter != null)
            {
                hintPrinter.PrintHintText(null); //Print nothing
            }
        }
    }

    public void ResetHintText()
    {
        hintPrinter.PrintHintText(null); //Print nothing
    }
}
