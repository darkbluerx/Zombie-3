using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("Drag the gameEvent")]
    [SerializeField] GameEvent activateTrap;
    [SerializeField] GameEvent deactiveTrap;
    [Space]

    [Header("How far you can open the door")]
    [SerializeField] bool FindSwitch = false;

    bool isWork = false;

    private void Update()
    {
        if (FindSwitch)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                if (!isWork)
                {
                    activateTrap.Raise(); // Raise the event to activate the trap
                    isWork = true;
                }
                else
                {
                    deactiveTrap.Raise(); // Raise the event to deactivate the trap
                    isWork = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindSwitch = true;
        }
    }
}
