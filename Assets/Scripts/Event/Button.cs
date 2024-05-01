using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Drag the gameEvent")]
    [SerializeField] GameEvent activateTrap;

    bool buttonPressed = false;
    internal object onClick;

    private void OnTriggerEnter(Collider other)
    {
        if(buttonPressed == false)
        {   
            AudioManager.Instance.GetTrapAudioEvent(); // Play buttonTrap sound
 
            activateTrap.Raise();
            StartCoroutine(ButtonAnimation());
        }
    }

    private IEnumerator ButtonAnimation()
    {
        buttonPressed = true;

        float orginalYpos = transform.position.y;

        while (transform.position.y > orginalYpos - 0.4f)
        {
            transform.Translate(Vector3.down * Time.deltaTime / 5);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
