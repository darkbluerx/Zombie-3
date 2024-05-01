using UnityEngine;

public class KeyPuzzle : MonoBehaviour
{
    [Header("How many keys to collect?")]
    [SerializeField] int numberOfKeysToCollect = 1;
    [Space]

    [Header("Drag the right event")]
    [SerializeField] GameEvent blueKeyEvent;
    [SerializeField] GameEvent greenKeyEvent;
    [SerializeField] GameEvent redKeyEvent;

    int keysCollected = 0;
    int eventNumber = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueKey"))
        {
            keysCollected++;
            Destroy(other.gameObject);
            Invoke("DisableHintText", 2.5f);

            if (keysCollected >= numberOfKeysToCollect && eventNumber == 0)
            {
                eventNumber++;
                blueKeyEvent.Raise();
                //Debug.Log("Blue key event raised");
            }
        }

        if(other.CompareTag("GreenKey"))
        {
            keysCollected++;
            Destroy(other.gameObject);
            Invoke("DisableHintText", 2.5f);

            if (keysCollected >= numberOfKeysToCollect && eventNumber == 1)
            {
                eventNumber++;
                greenKeyEvent.Raise();
                //Debug.Log("Green key event raised");
            }
        }

        if (other.CompareTag("RedKey"))
        {
            keysCollected++;
            Destroy(other.gameObject);
            Invoke("DisableHintText", 2.5f);

            if (keysCollected >= numberOfKeysToCollect && eventNumber == 2)
            {
                eventNumber++;
                redKeyEvent.Raise();
                //Debug.Log("Red key event raised");
            }
        }

        keysCollected = 0;
    }


    private void DisableHintText()
    {
       PlayerHint playerHint = FindObjectOfType<PlayerHint>();
       playerHint.ResetHintText(); //print nothing
    }
}
